using System;

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

        [Test]
        [TestCase("a", "a", 0)]
        [TestCase("A", "a", 0)]
        [TestCase("a", "A", 0)]
        [TestCase("a", "b", -1)]
        [TestCase("b", "a", +1)]
        public void Compare_BasicCases_ReturnsCorrectResults(string a, string b, int expected)
        {
            var comparer = new NaturalSortOrderStringComparer();

            var output = comparer.Compare(a, b);

            Assert.That(output, Is.EqualTo(expected));
        }


        [Test]
        [TestCase("0", "0", 0)]
        [TestCase("0", "1", -1)]
        [TestCase("1", "0", +1)]
        [TestCase("10", "8", +1)]
        public void Compare_OnlyBasicNumbers_ReturnsCorrectResults(string a, string b, int expected)
        {
            var comparer = new NaturalSortOrderStringComparer();

            var output = comparer.Compare(a, b);

            Assert.That(output, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("A10", "A10", 0)]
        [TestCase("A10", "A00010", -1)]
        [TestCase("A0010", "A010", +1)]
        [TestCase("A10", "A01000", -1)]
        public void Compare_CompoundStrings_ReturnsCorrectResults(string a, string b, int expected)
        {
            var comparer = new NaturalSortOrderStringComparer();

            var output = comparer.Compare(a, b);

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}