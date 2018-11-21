using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class StartingTokensFinderTest
    {
        public List<SingleToken> GetStartingTokens(string paragraph)
        {
            var finder = new StartingTokenFinder();
            var tokenTypes = new List<TokenType>
            {
                new TokenType(TokenTypeEnum.Lattice, "#", "h1", TokenLocationType.StartingToken),
                new TokenType(TokenTypeEnum.DoubleLattice, "##", "h2", TokenLocationType.StartingToken),
                new TokenType(TokenTypeEnum.TripleLattice, "###", "h3", TokenLocationType.StartingToken),
                new TokenType(TokenTypeEnum.QuadrupleLattice, "####", "h4", TokenLocationType.StartingToken),
                new TokenType(TokenTypeEnum.Star, "*", "li", TokenLocationType.StartingToken)
            };
            return finder.FindStartingTokens(paragraph, tokenTypes);
        }

        [TestCase("# word", new[] { 0 }, TestName = "Should find single lattice position")]
        [TestCase("# * word", new[] { 0, 2 }, TestName = "Should find lattice and star position")]
        [TestCase("# # word", new[] { 0 }, TestName = "Should not find double lattice position")]
        [TestCase("#  * word", new[] { 0, 3 }, TestName = "Should find lattice and star positions separated by double space")]
        public void CheckTokensPositions(string paragraph, int[] positions)
        {
            var tokens = GetStartingTokens(paragraph);
            tokens.Select(token => token.TokenPosition).ShouldAllBeEquivalentTo(positions);
        }

        [TestCase("# word", new[] { "Lattice" }, TestName = "Should find double single lattice name")]
        [TestCase("# * word", new[] { "Lattice", "Star" }, TestName = "Should find lattice and star name")]
        [TestCase("# # word", new[] { "Lattice" }, TestName = "Should not find double lattice name")]
        [TestCase("#  * word", new[] { "Lattice", "Star" }, TestName = "Should find lattice and star names separated by double space")]
        public void CheckTokensNames(string paragraph, string[] names)
        {
            var tokens = GetStartingTokens(paragraph);
            tokens.Select(token => token.TokenType.Name).ShouldAllBeEquivalentTo(names);
        }
    }
}
