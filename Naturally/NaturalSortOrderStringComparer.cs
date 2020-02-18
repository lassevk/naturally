using System;

using JetBrains.Annotations;

namespace Naturally
{
    [PublicAPI]
    public class NaturalSortOrderStringComparer : StringComparer
    {
        [PublicAPI]
        public static new readonly StringComparer CurrentCultureIgnoreCase =
            new NaturalSortOrderStringComparer();

#if !NETSTANDARD1_6
        [PublicAPI]
        public static new readonly StringComparer InvariantCultureIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparison.InvariantCultureIgnoreCase);
#endif

        #region These added just to make sure NaturalSortOrderStringComparer.XYZ produces the expected results
        
        [PublicAPI]
        public static new readonly StringComparer CurrentCulture =
            new NaturalSortOrderStringComparer(StringComparison.CurrentCulture);

#if !NETSTANDARD1_6
        [PublicAPI]
        public static new readonly StringComparer InvariantCulture =
            new NaturalSortOrderStringComparer(StringComparison.InvariantCulture);
#endif
        
        [PublicAPI]
        public static new readonly StringComparer Ordinal =
            new NaturalSortOrderStringComparer(StringComparison.Ordinal);
        
        [PublicAPI]
        public static new readonly StringComparer OrdinalIgnoreCase =
            new NaturalSortOrderStringComparer(StringComparison.OrdinalIgnoreCase);
        
        #endregion
        
        private readonly StringComparison _StringComparison;

        [PublicAPI]
        public NaturalSortOrderStringComparer(StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            _StringComparison = stringComparison;
        }

        [PublicAPI]
        public override bool Equals(string x, string y) => Compare(x, y) == 0;
        [PublicAPI]
        public override int GetHashCode(string obj) => throw new NotSupportedException();

        [PublicAPI]
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

            xs = Trim(xs, out bool xLeadingWhitespace, out bool xTrailingWhitespace);
            ys = Trim(ys, out bool yLeadingWhiteswpace, out bool yTrailingWhitespace);

            int result = CompareSection(xs, ys);
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
                    text = text.Slice(1);
            }

            trailingWhitespace = text.Length > 0 && Categorize(text[text.Length - 1]) == SectionCategory.Whitespace;
            if (trailingWhitespace)
            {
                while (text.Length > 0 && Categorize(text[text.Length - 1]) == SectionCategory.Whitespace)
                    text = text.Slice(0, text.Length - 1);
            }

            return text;
        }

        private int CompareSection(ReadOnlySpan<char> xs, ReadOnlySpan<char> ys)
        {
            int? sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces = null;

            while (xs.Length > 0 || ys.Length > 0)
            {
#if DEBUG
                int xLoopLength = xs.Length;
                int yLoopLength = ys.Length;
#endif

                xs = MoveNext(xs, out ReadOnlySpan<char> xSection, out SectionCategory xSectionCategory);
                ys = MoveNext(ys, out ReadOnlySpan<char> ySection, out SectionCategory ySectionCategory);

                if (xSectionCategory == SectionCategory.Whitespace && ySectionCategory == SectionCategory.Number)
                {
                    if (xs.Length > 0 && Categorize(xs[0]) == SectionCategory.Number)
                    {
                        ReadOnlySpan<char> nextXs = MoveNext(xs, out ReadOnlySpan<char> xNextSection, out SectionCategory xNextSectionCategory);
                        sortOrderBecauseOfNumericLengthOrLeadingOrTrailingSpaces ??=
                            SectionCategory.Whitespace.CompareTo(SectionCategory.Number);

                        xs = nextXs;
                        xSection = xNextSection;
                        xSectionCategory = xNextSectionCategory;
                    }
                }

                if (ySectionCategory == SectionCategory.Whitespace && xSectionCategory == SectionCategory.Number)
                {
                    if (ys.Length > 0 && Categorize(ys[0]) == SectionCategory.Number)
                    {
                        ReadOnlySpan<char> nextYs = MoveNext(ys, out ReadOnlySpan<char> yNextSection, out SectionCategory yNextSectionCategory);
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
                            int textComparisonResult = CompareTextSections(xSection, ySection);
                            if (textComparisonResult != 0)
                                return textComparisonResult;

                            break;

                        case SectionCategory.Number:
                            int numericComparisonResult = CompareNumericSections(xSection, ySection);
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

                int restLength = Math.Min(x.Length, y.Length);

                ReadOnlySpan<char> xNumber = x.Slice(x.Length - restLength);
                ReadOnlySpan<char> yNumber = y.Slice(y.Length - restLength);
                for (int index = 0; index < restLength; index++)
                {
                    double xValue = Char.GetNumericValue(xNumber[index]);
                    double yValue = Char.GetNumericValue(yNumber[index]);

                    int result = xValue.CompareTo(yValue);
                    if (result != 0)
                        return result;
                }

                return 0;
            }

            return x.CompareTo(y, _StringComparison);
        }

        private bool IsNonZero(ReadOnlySpan<char> number)
        {
            foreach (char digit in number)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Char.GetNumericValue(digit) != 0.0)
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
            for (int index = 1; index < text.Length; index++)
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
            if (Char.IsDigit(c))
                return SectionCategory.Number;

            if (Char.IsWhiteSpace(c))
                return SectionCategory.Whitespace;

            if (Char.IsPunctuation(c))
                return SectionCategory.Punctuation;

            return SectionCategory.Text;
        }
    }
}