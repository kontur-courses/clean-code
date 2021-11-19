using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void FindAll_ReturnsAllSubstring()
        {
            const string text = "abcdaefaga";

            var found = text.FindAll(new[] { "a", "b" }).ToArray();

            found.Should().BeEquivalentTo(new[] { ("a", 0), ("a", 4), ("a", 7), ("a", 9), ("b", 1) });
        }

        [Test]
        public void AllIndexesOf_ReturnAllIndexesOfSubstringInText()
        {
            var text = "_sadsad_sdad_  sad_";

            var found = text.AllIndexesOf("_").ToArray();

            found.Length.Should().Be(4);
            found.Should().BeEquivalentTo(new[] { 0, 7, 12, 18 });
        }
    }
}