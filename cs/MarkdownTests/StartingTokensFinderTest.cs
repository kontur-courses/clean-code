using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class StartingTokensFinderTest
    {
        [TestCase("# word", new[] { 0 }, TestName = "Should find single lattice position")]
        [TestCase("# # word", new[] { 0, 2}, TestName = "Should find double single lattice position")]
        public void CheckTokensPositions(string paragraph, int[] positions)
        {
            var finder = new StartingTokenFinder();

            var tokens = finder.FindStartingTokens(paragraph);
            tokens.Select(token => token.TokenPosition).ShouldAllBeEquivalentTo(positions);
        }

        [TestCase("# word", new[] { "lattice" }, TestName = "Should find double single lattice name")]
        [TestCase("# # word", new[] { "lattice", "lattice" }, TestName = "Should find single lattice name")]
        public void CheckTokensNames(string paragraph, string[] names)
        {
            var finder = new StartingTokenFinder();

            var tokens = finder.FindStartingTokens(paragraph);
            tokens.Select(token => token.TokenType.Name).ShouldAllBeEquivalentTo(names);
        }
    }
}
