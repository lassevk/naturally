using System;
using System.Collections.Generic;
using System.Linq;

namespace Naturally
{
    public class NaturalSortOrderStringComparer : StringComparer
    {
        public static new readonly StringComparer CurrentCulture =
            new NaturalSortOrderStringComparer(StringComparer.CurrentCulture);

        public static new readonly StringComparer CurrentCultureIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparer.CurrentCultureIgnoreCase);

        public static new readonly StringComparer InvariantCultureIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparer.InvariantCultureIgnoreCase);

        public static new readonly StringComparer InvariantCulture =
            new NaturalSortOrderStringComparer(StringComparer.InvariantCulture);
        
        public static new readonly StringComparer Ordinal =
            new NaturalSortOrderStringComparer(StringComparer.Ordinal);
        
        public static new readonly StringComparer OrdinalIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparer.OrdinalIgnoreCase);
        
        private static readonly Dictionary<char, double> _DigitValue = new Dictionary<char, double>();

        private static readonly Dictionary<(SectionCategory x, SectionCategory y), int> _SectionDifferencesResults =
            new Dictionary<(SectionCategory x, SectionCategory y), int>
            {
                [(SectionCategory.Empty, SectionCategory.Number)] = -1, // number after nothing
                [(SectionCategory.Empty, SectionCategory.Text)] = -1, // text after nothing
                [(SectionCategory.Empty, SectionCategory.Whitespace)] = -1, // whitespace after nothing
                [(SectionCategory.Empty, SectionCategory.Punctuation)] = -1, // punctuation after nothing

                [(SectionCategory.Number, SectionCategory.Text)] = -1, // number before text 
                [(SectionCategory.Number, SectionCategory.Whitespace)] = -1, // number before whitespace
                [(SectionCategory.Number, SectionCategory.Punctuation)] = +1, // number after punctuation

                [(SectionCategory.Text, SectionCategory.Whitespace)] = +1, // whitespace before text
                [(SectionCategory.Text, SectionCategory.Punctuation)] = -1, // text before punctuation

                [(SectionCategory.Whitespace, SectionCategory.Punctuation)] = +1, // punctuation before whitespace
            };

        static NaturalSortOrderStringComparer()
        {
            SectionCategory[] categories = Enum.GetValues(typeof(SectionCategory)).OfType<SectionCategory>().ToArray();
            var additions = new List<(SectionCategory x, SectionCategory y, int result)>();

            foreach (SectionCategory x in categories)
                foreach (SectionCategory y in categories)
                {
                    if (x == y)
                        continue;

                    if (_SectionDifferencesResults.TryGetValue((x, y), out var result))
                    {
                        if (_SectionDifferencesResults.TryGetValue((y, x), out _))
                            throw new InvalidOperationException($"Both {x}-{y} and {y}-{x} defined in internal dictionary");

                        additions.Add((y, x, -result));
                    }
                }

            foreach ((SectionCategory x, SectionCategory y, int result) addition in additions)
                _SectionDifferencesResults.Add((addition.x, addition.y), addition.result);

            foreach (SectionCategory x in categories)
                foreach (SectionCategory y in categories)
                {
                    if (x == y)
                        continue;

                    if (!_SectionDifferencesResults.TryGetValue((x, y), out _))
                        throw new InvalidOperationException($"{x}-{y} missing from section category results");
                }

            IEnumerable<char> digits = Enumerable.Range(0, 65536).Select(i => (char)i);
            foreach (var digit in digits)
            {
                var numericValue = Char.GetNumericValue(digit);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (numericValue != -1)
                    _DigitValue[digit] = numericValue;
            }
        }

        private readonly StringComparer _TextStringComparer;

        public NaturalSortOrderStringComparer(StringComparer textStringComparer = null)
        {
            _TextStringComparer = textStringComparer ?? CurrentCultureIgnoreCase;
        }

        public override bool Equals(string x, string y) => Compare(x, y) == 0;
        public override int GetHashCode(string obj) => throw new NotSupportedException();

        public override int Compare(string x, string y)
        {
            if (ReferenceEquals(x, y))
                return 0;

            if (x == null)
                return -1;

            if (y == null)
                return +1;

            ReadOnlySpan<char> xs = x.AsSpan();
            ReadOnlySpan<char> ys = y.AsSpan();

            xs = Trim(xs, out var xLeadingWhitespace, out var xTrailingWhitespace);
            ys = Trim(ys, out var yLeadingWhiteswpace, out var yTrailingWhitespace);

            var result = CompareSection(xs, ys);
            if (result != 0)
                return result;

            // leading whitespace after non-leading whitespace
            if (xLeadingWhitespace && !yLeadingWhiteswpace)
                return +1;

            if (!xLeadingWhitespace && yLeadingWhiteswpace)
                return -1;

            // trailing whitespace after non-trailing whitespace
            if (xTrailingWhitespace && !yTrailingWhitespace)
                return +1;

            if (!xTrailingWhitespace && yTrailingWhitespace)
                return -1;

            // Leading/trailing is equal 
            return 0;
        }

        private ReadOnlySpan<char> Trim(ReadOnlySpan<char> text, out bool leadingWhitespace, out bool trailingWhitespace)
        {
            if (text.Length == 0)
            {
                leadingWhitespace = false;
                trailingWhitespace = false;
                return text;
            }

            leadingWhitespace = Categorize(text[0]) == SectionCategory.Whitespace;
            if (leadingWhitespace)
            {
                while (text.Length > 0 && Categorize(text[0]) == SectionCategory.Whitespace)
                    text = text[1..];
            }

            trailingWhitespace = text.Length > 0 && Categorize(text[^1]) == SectionCategory.Whitespace;
            if (trailingWhitespace)
            {
                while (text.Length > 0 && Categorize(text[^1]) == SectionCategory.Whitespace)
                    text = text[..^1];
            }

            return text;
        }

        private int CompareSection(ReadOnlySpan<char> xs, ReadOnlySpan<char> ys)
        {
            int? sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces = null;

            while (xs.Length > 0 || ys.Length > 0)
            {
#if DEBUG
                var xLoopLength = xs.Length;
                var yLoopLength = ys.Length;
#endif

                xs = MoveNext(xs, out ReadOnlySpan<char> xSection, out SectionCategory xSectionCategory);
                ys = MoveNext(ys, out ReadOnlySpan<char> ySection, out SectionCategory ySectionCategory);

                if (xSectionCategory == SectionCategory.Whitespace && ySectionCategory == SectionCategory.Number)
                {
                    ReadOnlySpan<char> nextXs = MoveNext(xs, out ReadOnlySpan<char> xNextSection, out SectionCategory xNextSectionCategory);
                    if (xNextSectionCategory == SectionCategory.Number)
                    {
                        sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??=
                            _SectionDifferencesResults[(SectionCategory.Whitespace, SectionCategory.Number)];

                        xs = nextXs;
                        xSection = xNextSection;
                        xSectionCategory = xNextSectionCategory;
                    }
                }

                if (ySectionCategory == SectionCategory.Whitespace && xSectionCategory == SectionCategory.Number)
                {
                    ReadOnlySpan<char> nextYs = MoveNext(ys, out ReadOnlySpan<char> yNextSection, out SectionCategory yNextSectionCategory);
                    if (yNextSectionCategory == SectionCategory.Number)
                    {
                        sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??=
                            _SectionDifferencesResults[(SectionCategory.Number, SectionCategory.Whitespace)];

                        ;
                        ys = nextYs;
                        ySection = yNextSection;
                        ySectionCategory = yNextSectionCategory;
                    }
                }

                if (xSectionCategory == ySectionCategory)
                {
                    switch (xSectionCategory)
                    {
                        case SectionCategory.Empty:
                            break;

                        case SectionCategory.Whitespace:
                            sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??= xSection.Length.CompareTo(ySection.Length);
                            break;

                        case SectionCategory.Text:
                        case SectionCategory.Punctuation:
                            var textComparisonResult = CompareTextSections(xSection, ySection);
                            if (textComparisonResult != 0)
                                return textComparisonResult;

                            break;

                        case SectionCategory.Number:
                            var numericComparisonResult = CompareNumericSections(xSection, ySection);
                            if (numericComparisonResult != 0)
                                return numericComparisonResult;

                            sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??= xSection.Length.CompareTo(ySection.Length);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    if (_SectionDifferencesResults.TryGetValue((xSectionCategory, ySectionCategory), out var sectionResult))
                        if (sectionResult != 0)
                            return sectionResult;
                }

#if DEBUG
                if (xs.Length == xLoopLength && ys.Length == yLoopLength)
                    throw new InvalidOperationException("Internal error, comparison loop got stuck");
#endif
            }

            return sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ?? 0;
        }

        private int CompareTextSections(in ReadOnlySpan<char> x, in ReadOnlySpan<char> y)
            => _TextStringComparer.Compare(x.ToString(), y.ToString());

        private int CompareNumericSections(in ReadOnlySpan<char> x, in ReadOnlySpan<char> y)
        {
            if (x.Length != y.Length)
            {
                if (x.Length > y.Length)
                {
                    if (IsNonZero(x.Slice(0, x.Length - y.Length)))
                        return 1;
                }
                else if (IsNonZero(y.Slice(0, y.Length - x.Length)))
                    return -1;

                var restLength = Math.Min(x.Length, y.Length);

                ReadOnlySpan<char> xNumber = x[^restLength..];
                ReadOnlySpan<char> yNumber = y[^restLength..];
                for (var index = 0; index < restLength; index++)
                {
                    if (!_DigitValue.TryGetValue(xNumber[index], out var xValue))
                        throw new InvalidOperationException($"Internal error, unknown digit '{xNumber[index]}'");

                    if (!_DigitValue.TryGetValue(yNumber[index], out var yValue))
                        throw new InvalidOperationException($"Internal error, unknown digit '{yNumber[index]}'");

                    var result = xValue.CompareTo(yValue);
                    if (result != 0)
                        return result;
                }

                return 0;
            }

            return _TextStringComparer.Compare(x.ToString(), y.ToString());
        }

        private bool IsNonZero(ReadOnlySpan<char> number)
        {
            foreach (var digit in number)
            {
                if (!_DigitValue.TryGetValue(digit, out var order))
                    throw new InvalidOperationException($"Internal error, unknown digit '{digit}'");

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (order != 0.0)
                    return true;
            }

            return false;
        }

        private ReadOnlySpan<char> MoveNext(ReadOnlySpan<char> text, out ReadOnlySpan<char> section, out SectionCategory category)
        {
            if (text.Length == 0)
            {
                section = text;
                category = SectionCategory.Empty;
                return text;
            }

            category = Categorize(text[0]);
            for (var index = 1; index < text.Length; index++)
                if (Categorize(text[index]) != category)
                {
                    section = text.Slice(0, index);
                    return text.Slice(index);
                }

            section = text;
            return ReadOnlySpan<char>.Empty;
        }

        private static SectionCategory Categorize(char c)
        {
            if (_DigitValue.ContainsKey(c))
                return SectionCategory.Number;

            if (Char.IsWhiteSpace(c))
                return SectionCategory.Whitespace;

            if (Char.IsPunctuation(c))
                return SectionCategory.Punctuation;

            return SectionCategory.Text;
        }
    }
}