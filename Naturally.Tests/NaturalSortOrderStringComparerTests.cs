using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace Naturally.Tests
{
    [TestFixture]
    public class NaturalSortOrderStringComparerTests
    {
        private static IEnumerable<TestCaseData> Compare_TestCases()
        {
            StringComparison sc = StringComparison.InvariantCultureIgnoreCase;
            
            yield return new TestCaseData(null, null, sc, 0).SetName("null equals null");
            yield return new TestCaseData(null, "", sc, -1).SetName("null before empty string");
            yield return new TestCaseData(null, "a", sc, -1).SetName("null before a");

            yield return new TestCaseData("a", "", sc, +1).SetName("Empty string before a");
            
            yield return new TestCaseData("a", "a", sc, 0).SetName("Lower-case a compares equal a");
            yield return new TestCaseData("a", "A", sc, 0).SetName("Case-insensitive a equals A");
            yield return new TestCaseData("a", "A", StringComparison.InvariantCulture, -1).SetName("Case sensitive a before A");
            yield return new TestCaseData("A", "a", StringComparison.InvariantCulture, +1).SetName("Case sensitive A after a");
            yield return new TestCaseData("a", "b", sc, -1).SetName("a comes before b");

            yield return new TestCaseData("0", "0", sc, 0).SetName("0 equals 0");
            yield return new TestCaseData("1", "1", sc, 0).SetName("1 equals 1");
            yield return new TestCaseData("0", "1", sc, -1).SetName("0 before 1");
            yield return new TestCaseData("10", "8", sc, +1).SetName("8 before 10");
            
            yield return new TestCaseData("A10", "A10", sc, 0).SetName("A10 equals A10");
            yield return new TestCaseData("A10", "A11", sc, -1).SetName("A10 before A11");
            yield return new TestCaseData("A10", "A010", sc, -1).SetName("A10 before A010");
            yield return new TestCaseData("A10B20", "A010B19", sc, +1).SetName("A10B20 after A010B19 because B19<B20");

            yield return new TestCaseData(" a", "a", sc, +1).SetName("Leading spaces before non-leading spaces");
            yield return new TestCaseData(" a10", "a11", sc, -1).SetName("Only consider leading spaces if all other comparisons fail");
            yield return new TestCaseData("a10", "a10 ", sc, -1).SetName("Non-trailing whitespace after trailing whitespace");
            yield return new TestCaseData("a11", "a10 ", sc, +1).SetName("Only consider trailing spaces if all other comparisons fail");

            yield return new TestCaseData("a b", "a  b", sc, -1).SetName("Amount of whitespaces matter");
            yield return new TestCaseData("a b", "a  a", sc, +1).SetName("Amount of whitespaces does not matter if other more important rules kick in first");

            yield return new TestCaseData("\u00bd", "0", sc, +1).SetName("1/2 after 0");
            yield return new TestCaseData("\u2153", "0", sc, +1).SetName("1/3 after 0");
            yield return new TestCaseData("\u215d", "\u2158", sc, -1).SetName("5/8 before 4/5");

            yield return new TestCaseData("Test1", "Test", sc, +1).SetName("Text without number before text with");
            yield return new TestCaseData("Test1.txt", "Test.txt", sc, +1).SetName("Text without number before text with (.txt)");

            yield return new TestCaseData("A11", "A 10", sc, +1).SetName("Ignore whitespace before numbers if numbers make a difference #1");
            yield return new TestCaseData("A10", "A 11", sc, -1).SetName("Ignore whitespace before numbers if numbers make a difference #2");
            yield return new TestCaseData("A10", "A 10", sc, -1).SetName("Do not ignore whitespace before numbers when numbers don't make a difference");

            yield return new TestCaseData("ss", "\u00df", sc, 0).SetName("ss equal to \u00df");
            yield return new TestCaseData("\u00df", "ss", sc, 0).SetName("\u00df equal to ss");
        }

        [TestCaseSource(nameof(Compare_TestCases))]
        public void Compare_WithTestCases_ReturnsExpectedResults(string a, string b, StringComparison comparison, int expected)
        {
            NaturalSortOrderStringComparer comparer = new NaturalSortOrderStringComparer(comparison);

            int output = comparer.Compare(a, b);

            Assert.That(output, Is.EqualTo(expected));

            if (expected != 0 && output == expected)
            {
                output = comparer.Compare(b, a);
                Assert.That(output, Is.EqualTo(-expected), "Stability-check");
            }
        }

        [Test]
        public void Fuzzing()
        {
            Random r = new Random(12345);

            string randomText()
            {
                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < r.Next(50) + 1; index++)
                    sb.Append((char)r.Next(65536));

                return sb.ToString();
            }

            List<string> strings = new List<string> { null, "", "a", "aaaaaaaaaaaaaaaaaaaa" };
            for (int index = 0; index < 1000; index++)
                strings.Add(randomText());
            
            foreach (string x in strings)
                foreach (string y in strings)
                {
                    int result = NaturalSortOrderStringComparer.InvariantCultureIgnoreCase.Compare(x, y);
                    Assert.That(result >= -1 && result <= 1);
                }
        }
    }
}