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
            const string text = "abcdaefagaa";

            var found = text.FindAll(new[] { "a", "b", "aa" }).ToArray().Select(t=>(t.Start, t.Length));

            found.Should().BeEquivalentTo(new[] { (0, 1), (4, 1), (7, 1), (9, 2), (1, 1) });
        }

        [Test]
        public void AllIndexesOf_ReturnAllIndexesOfSubstringInText()
        {
            const string text = "_sadsad_sdad_  sad_";

            var found = text.AllIndexesOf("_").ToArray();

            found.Length.Should().Be(4);
            found.Should().BeEquivalentTo(new[] { 0, 7, 12, 18 });
        }
    }
}