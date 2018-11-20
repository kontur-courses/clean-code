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
        [TestCase("# * word", new[] { 0, 2 }, TestName = "Should find lattice and star position")]
        [TestCase("# # word", new[] { 0 }, TestName = "Should not find double lattice position")]
        [TestCase("#  * word", new[] { 0, 3 }, TestName = "Should find lattice and star positions separated by double space")]
        public void CheckTokensPositions(string paragraph, int[] positions)
        {
            var finder = new StartingTokenFinder();

            var tokens = finder.FindStartingTokens(paragraph);
            tokens.Select(token => token.TokenPosition).ShouldAllBeEquivalentTo(positions);
        }

        [TestCase("# word", new[] { "lattice" }, TestName = "Should find double single lattice name")]
        [TestCase("# * word", new[] { "lattice", "star" }, TestName = "Should find lattice and star name")]
        [TestCase("# # word", new[] { "lattice" }, TestName = "Should not find double lattice name")]
        [TestCase("#  * word", new[] { "lattice", "star" }, TestName = "Should find lattice and star names separated by double space")]
        public void CheckTokensNames(string paragraph, string[] names)
        {
            var finder = new StartingTokenFinder();

            var tokens = finder.FindStartingTokens(paragraph);
            tokens.Select(token => token.TokenType.Name).ShouldAllBeEquivalentTo(names);
        }
    }
}
