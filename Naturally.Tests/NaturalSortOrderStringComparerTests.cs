using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Naturally.Tests
{
    [TestFixture]
    public class NaturalSortOrderStringComparerTests
    {
        [Test]
        public void Constructor_NullStringComparer_DoesNotThrowNullReferenceException()
        {
            Assert.DoesNotThrow(() => new NaturalSortOrderStringComparer(null));
        }

        public static IEnumerable<TestCaseData> Compare_TestCases()
        {
            yield return new TestCaseData("a", "a", null, 0).SetName("Lower-case a compares equal a");
            yield return new TestCaseData("a", "A", null, 0).SetName("Case-insensitive a equals A");
            yield return new TestCaseData("a", "A", StringComparer.InvariantCulture, -1).SetName("Not case sensitive a before A");
            yield return new TestCaseData("A", "a", StringComparer.InvariantCulture, +1).SetName("Not case sensitive A after a");
            yield return new TestCaseData("a", "b", null, -1).SetName("a comes before b");

            yield return new TestCaseData("0", "0", null, 0).SetName("0 equals 0");
            yield return new TestCaseData("1", "1", null, 0).SetName("1 equals 1");
            yield return new TestCaseData("0", "1", null, -1).SetName("0 before 1");
            yield return new TestCaseData("10", "8", null, +1).SetName("8 before 10");
            
            yield return new TestCaseData("A10", "A10", null, 0).SetName("A10 equals A10");
            yield return new TestCaseData("A10", "A11", null, -1).SetName("A10 before A11");
            yield return new TestCaseData("A10", "A010", null, -1).SetName("A10 before A010");
            yield return new TestCaseData("A10B20", "A010B19", null, +1).SetName("A10B20 after A010B19 because B19<B20");

            yield return new TestCaseData(" a", "a", null, +1).SetName("Leading spaces before non-leading spaces");
            yield return new TestCaseData(" a10", "a11", null, -1).SetName("Only consider leading spaces if all other comparisons fail");
            yield return new TestCaseData("a10", "a10 ", null, -1).SetName("Non-trailing whitespace after trailing whitespace");
            yield return new TestCaseData("a11", "a10 ", null, +1).SetName("Only consider trailing spaces if all other comparisons fail");

            yield return new TestCaseData("a b", "a  b", null, -1).SetName("Amount of whitespaces matter");
            yield return new TestCaseData("a b", "a  a", null, +1).SetName("Amount of whitespaces does not matter if other more important rules kick in first");
        }

        [TestCaseSource(nameof(Compare_TestCases))]
        public void Compare_WithTestCases_ReturnsExpectedResults(string a, string b, StringComparer textStringComparer, int expected)
        {
            var comparer = new NaturalSortOrderStringComparer(textStringComparer);

            var output = comparer.Compare(a, b);

            Assert.That(output, Is.EqualTo(expected));

            if (expected != 0 && output == expected)
            {
                output = comparer.Compare(b, a);
                Assert.That(output, Is.EqualTo(-expected), "Stability-check");
            }
        }
    }
}