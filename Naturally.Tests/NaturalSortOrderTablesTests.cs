using System;

using NUnit.Framework;

namespace Naturally.Tests
{
    [TestFixture]
    public class NaturalSortOrderTablesTests
    {
        [Test]
        public void NumericValues_ContainsCorrectValues()
        {
            for (int index = 0; index < 65536; index++)
            {
                char c = (char)index;
                double value = Char.GetNumericValue(c);

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (value == -1)
                {
                    if (NaturalSortOrderTables.NumericValues.ContainsKey(c))
                        throw new InvalidOperationException($"Lookup table contains value for {index:X4}, but shouldn't");
                }
                else
                {
                    if (!NaturalSortOrderTables.NumericValues.ContainsKey(c))
                        throw new InvalidOperationException(
                            $"Lookup table does not contain value for {index:X4}, but it should, value is {value:R19}");

                    double existingValue = NaturalSortOrderTables.NumericValues[c];

                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (existingValue != value)
                        throw new InvalidOperationException(
                            $"Lookup table contains the wrong value for {index:X4}, reported value from .NET is {value:R19}, dictionary contains {existingValue:R19}");
                }
            }
        }
    }
}