using System;
using System.Collections.Generic;
using System.Globalization;

namespace Naturally
{
    public class NaturalSortOrderStringComparer : StringComparer
    {
        private readonly StringComparer _textStringComparer;

        public NaturalSortOrderStringComparer(StringComparer textStringComparer = null)
        {
            _textStringComparer = textStringComparer ?? CurrentCultureIgnoreCase;
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

            return CompareSection(xs, ys);
        }

        private int CompareSection(ReadOnlySpan<char> xs, ReadOnlySpan<char> ys)
        {
            ReadOnlySpan<char> xSection;
            ReadOnlySpan<char> ySection;
            bool xSectionIsNumeric;
            bool ySectionIsNumeric;
            
            while (xs.Length > 0 || ys.Length > 0)
            {
#if DEBUG
                int xLoopLength = xs.Length;
                int yLoopLength = ys.Length;
#endif
                
                xs = MoveNext(xs, out xSection, out xSectionIsNumeric);
                ys = MoveNext(ys, out ySection, out ySectionIsNumeric);

                int sectionComparisonResult;
                if (xSectionIsNumeric && ySectionIsNumeric)
                    sectionComparisonResult = CompareNumericSections(xSection, ySection);
                else if (xSectionIsNumeric)
                    sectionComparisonResult = +1;
                else if (ySectionIsNumeric)
                    sectionComparisonResult = -1;
                else
                    sectionComparisonResult = CompareTextSections(xSection, ySection);

                if (sectionComparisonResult != 0)
                    return sectionComparisonResult;

#if DEBUG
                if (xs.Length == xLoopLength && ys.Length == yLoopLength)
                    throw new InvalidOperationException("Internal error, comparison loop got stuck");
#endif
            }

            return 0;
        }

        private int CompareTextSections(in ReadOnlySpan<char> x, in ReadOnlySpan<char> y) => _textStringComparer.Compare(x.ToString(), y.ToString());

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
                int result = _textStringComparer.Compare(x[^restLength..].ToString(), y[^restLength..].ToString());
                if (result != 0)
                    return result;

                return x.Length.CompareTo(y.Length);
            }

            return _textStringComparer.Compare(x.ToString(), y.ToString());
        }

        private bool IsNonZero(ReadOnlySpan<char> number)
        {
            foreach (char digit in number)
                if (digit != '0')
                    return true;

            return false;
        }

        private ReadOnlySpan<char> MoveNext(ReadOnlySpan<char> text, out ReadOnlySpan<char> section, out bool sectionIsNumeric)
        {
            if (text.Length == 0)
            {
                section = text;
                sectionIsNumeric = false;
                return text;
            }

            sectionIsNumeric = IsDigit(text[0]);
            for (int index = 1; index < text.Length; index++)
                if (IsDigit(text[index]) != sectionIsNumeric)
                {
                    section = text.Slice(0, index);
                    return text.Slice(index);
                }

            section = text;
            return ReadOnlySpan<char>.Empty;
        }

        private static bool IsDigit(char c) => Char.GetUnicodeCategory(c) == UnicodeCategory.DecimalDigitNumber;
    }
}