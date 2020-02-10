using System;

namespace Naturally
{
    public class NaturalSortOrderStringComparer : StringComparer
    {
        public static new readonly StringComparer CurrentCultureIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparison.CurrentCultureIgnoreCase);

        public static new readonly StringComparer InvariantCultureIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparison.InvariantCultureIgnoreCase);

        #region These added just to make sure NaturalSortOrderStringComparer.XYZ produces the expected results
        
        public static new readonly StringComparer CurrentCulture =
            new NaturalSortOrderStringComparer(StringComparison.CurrentCulture);

        public static new readonly StringComparer InvariantCulture =
            new NaturalSortOrderStringComparer(StringComparison.InvariantCulture);
        
        public static new readonly StringComparer Ordinal =
            new NaturalSortOrderStringComparer(StringComparison.Ordinal);
        
        public static new readonly StringComparer OrdinalIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparison.OrdinalIgnoreCase);
        
        #endregion
        
        private readonly StringComparison _StringComparison;

        public NaturalSortOrderStringComparer(StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            _StringComparison = stringComparison;
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
                            SectionCategory.Whitespace.CompareTo(SectionCategory.Number);

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
                            SectionCategory.Number.CompareTo(SectionCategory.Whitespace);

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
                    return xSectionCategory.CompareTo(ySectionCategory);
                }

#if DEBUG
                if (xs.Length == xLoopLength && ys.Length == yLoopLength)
                    throw new InvalidOperationException("Internal error, comparison loop got stuck");
#endif
            }

            return sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ?? 0;
        }

        private int CompareTextSections(in ReadOnlySpan<char> x, in ReadOnlySpan<char> y) => x.CompareTo(y, _StringComparison);

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
                    if (!NaturalSortOrderTables.NumericValues.TryGetValue(xNumber[index], out var xValue))
                        throw new InvalidOperationException($"Internal error, unknown digit '{xNumber[index]}'");

                    if (!NaturalSortOrderTables.NumericValues.TryGetValue(yNumber[index], out var yValue))
                        throw new InvalidOperationException($"Internal error, unknown digit '{yNumber[index]}'");

                    var result = xValue.CompareTo(yValue);
                    if (result != 0)
                        return result;
                }

                return 0;
            }

            return x.CompareTo(y, _StringComparison);
        }

        private bool IsNonZero(ReadOnlySpan<char> number)
        {
            foreach (var digit in number)
            {
                NaturalSortOrderTables.NumericValues.TryGetValue(digit, out var value);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value != 0.0)
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
            if (NaturalSortOrderTables.NumericValues.ContainsKey(c))
                return SectionCategory.Number;

            if (Char.IsWhiteSpace(c))
                return SectionCategory.Whitespace;

            if (Char.IsPunctuation(c))
                return SectionCategory.Punctuation;

            return SectionCategory.Text;
        }
    }
}