using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class EscapeFinderTests
    {
        private readonly EscapeFinder escapeFinder = new ();

        [TestCase("")]
        [TestCase("abc")]
        [TestCase("_a_")]
        public void FindEscapePositions_ShouldReturnEmpty_WhenTextHasNoBackslashes_Test(string text)
        {
            var positions = escapeFinder.FindEscapePositions(text);

            positions.Should().BeEmpty();
        }

        [TestCase(@"\_a", new[] { 0 })]
        [TestCase(@"a\\b", new[] { 1 })]
        [TestCase(@"\_a\_", new[] { 0, 3 })]
        public void FindEscapePositions_ShouldFindBackslash_WhenItIsBeforeMarkupSymbol_Test(
            string text,
            int[] expectedPositions)
        {
            var positions = escapeFinder.FindEscapePositions(text);

            positions.Should().BeEquivalentTo(expectedPositions);
        }

        [TestCase(@"\\_", new[] { 0 })]
        [TestCase(@"\\\_", new[] { 0, 2 })]
        public void FindEscapePositions_ShouldSkipBackslash_WhenItIsEscaped_Test(string text, int[] expectedPositions)
        {
            var positions = escapeFinder.FindEscapePositions(text);

            positions.Should().BeEquivalentTo(expectedPositions);
        }

        [TestCase(@"\a")]
        [TestCase(@"\ a")]
        [TestCase(@"\#")]
        [TestCase(@"a\")]
        public void FindEscapePositions_ShouldReturnEmpty_WhenBackslashIsNotBeforeMarkupSymbol_Test(string text)
        {
            var positions = escapeFinder.FindEscapePositions(text);

            positions.Should().BeEmpty();
        }
    }
}
