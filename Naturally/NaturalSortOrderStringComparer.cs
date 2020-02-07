using System;
using System.Collections.Generic;
using System.Linq;

namespace Naturally
{
    public class NaturalSortOrderStringComparer : StringComparer
    {
        private static readonly Dictionary<char, int> _DigitOrder = new Dictionary<char, int>();
        private static readonly Dictionary<(SectionCategory x, SectionCategory y), int> _SectionDifferencesResults =
            new Dictionary<(SectionCategory x, SectionCategory y), int>
            {
                [(SectionCategory.Empty, SectionCategory.Number)] = -1, // number after nothing
                [(SectionCategory.Empty, SectionCategory.Text)] = -1, // text after nothing
                [(SectionCategory.Empty, SectionCategory.Whitespace)] = -1, // whitespace after nothing
                
                [(SectionCategory.Number, SectionCategory.Text)] = +1, // text without number before text with 
                [(SectionCategory.Number, SectionCategory.Whitespace)] = +1, // whitespace before number
                
                [(SectionCategory.Text, SectionCategory.Whitespace)] = +1, // whitespace before text
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

            char[] digits =
            {
                '0',
                '\u2189', // 0/3  = 0
                '\u2152', // 1/10 = 0.1
                '\u2151', // 1/9  = 0.111
                '\u215b', // 1/8  = 0.125
                '\u2150', // 1/7  = 0.142
                '\u2159', // 1/6  = 0.166
                '\u2155', // 1/5  = 0.2
                '\u00bc', // 1/4  = 0.25
                '\u2153', // 1/3  = 0.333
                '\u215c', // 3/8  = 0.375
                '\u2156', // 2/5  = 0.4
                '\u00bd', // 1/2  = 0.5
                '\u2157', // 3/5  = 0.6
                '\u215d', // 5/8  = 0.625
                '\u2154', // 2/3  = 0.666
                '\u00be', // 3/4  = 0.75
                '\u2158', // 4/5  = 0.8
                '\u215a', // 5/6  = 0.833
                '\u215e', // 7/8  = 0.875
                '1', '2', '3', '4', '5', '6', '7', '8', '9'
            };
            for (int index = 0; index < digits.Length; index++)
                _DigitOrder[digits[index]] = index;
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
            // if (ReferenceEquals(x, y))
            //     return 0;
            // if (x == null)
            //     return -1;
            // if (y == null)
            //     return +1;
            
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
                    if (xSectionCategory == SectionCategory.Empty)
                        sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??= +1;
                    else if (ySectionCategory == SectionCategory.Empty)
                        sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??= -1;
                    
                }

#if DEBUG
                if (xs.Length == xLoopLength && ys.Length == yLoopLength)
                    throw new InvalidOperationException("Internal error, comparison loop got stuck");
#endif
            }

            return sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ?? 0;
        }

        private int CompareTextSections(in ReadOnlySpan<char> x, in ReadOnlySpan<char> y) => _TextStringComparer.Compare(x.ToString(), y.ToString());

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
                for (int index = 0; index < restLength; index++)
                {
                    if (!_DigitOrder.TryGetValue(xNumber[index], out var xOrder))
                        throw new InvalidOperationException($"Internal error, unknown digit '{xNumber[index]}'");
                    if (!_DigitOrder.TryGetValue(yNumber[index], out var yOrder))
                        throw new InvalidOperationException($"Internal error, unknown digit '{yNumber[index]}'");

                    if (xOrder != yOrder)
                        return xOrder.CompareTo(yOrder);
                }

                return 0;
            }

            return _TextStringComparer.Compare(x.ToString(), y.ToString());
        }

        private bool IsNonZero(ReadOnlySpan<char> number)
        {
            foreach (var digit in number)
            {
                if (!_DigitOrder.TryGetValue(digit, out var order))
                    throw new InvalidOperationException($"Internal error, unknown digit '{digit}'");
                
                if (order != 0)
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
            if (_DigitOrder.ContainsKey(c))
                return SectionCategory.Number;

            if (Char.IsWhiteSpace(c))
                return SectionCategory.Whitespace;

            return SectionCategory.Text;
        }
    }
}