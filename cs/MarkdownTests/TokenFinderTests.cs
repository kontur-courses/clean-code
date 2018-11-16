using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class TokenFinderTests
    {
        [TestCase("__f _d_ f__", new[] { 4, 6 }, new[] { 0, 9 }, TestName = "Should find simple and double token")]
        public void FindDoubleAndSimple(string paragraph, int[] simpleUnderscorePositions,
            int[] doubleUnderscorePositions)
        {
            var expectedDoubleUnderscorePositions = new List<TokenPosition>();
            var expectedSimpleUnderscorePositions = new List<TokenPosition>();
            for (var i = 0; i < simpleUnderscorePositions.Length; i += 2)
                expectedSimpleUnderscorePositions.Add(new TokenPosition(simpleUnderscorePositions[i],
                    simpleUnderscorePositions[i + 1]));
            for (var i = 0; i < doubleUnderscorePositions.Length; i += 2)
                expectedDoubleUnderscorePositions.Add(new TokenPosition(doubleUnderscorePositions[i],
                    doubleUnderscorePositions[i + 1]));

            var finder = new TokenFinder();
            var tokensWithPositions = finder.GetTokensWithPositions(paragraph);

            tokensWithPositions.First(token => token.Key.Name == "simpleUnderscore").Value.ShouldBeEquivalentTo(expectedSimpleUnderscorePositions);
            tokensWithPositions.First(token => token.Key.Name == "doubleUnderscore").Value.ShouldBeEquivalentTo(expectedDoubleUnderscorePositions);
        }

        [Test]
        public void ShouldFindDoubleUnderscore()
        {
            var tokenFinder = new TokenFinder();

            var paragraph = "__f__";
            var tokensWithPositions = tokenFinder.GetTokensWithPositions(paragraph);

            tokensWithPositions.First(token => token.Key.Name == "doubleUnderscore").Value.First().ShouldBeEquivalentTo(new TokenPosition(0, 3));
        }

        [TestCase("_ff\\_", TestName = "Should not find finishing token with screening")]
        [TestCase("\\_ff_", TestName = "Should not find starting token with screening")]
        [TestCase("f_", TestName = "Should not find token without starting token")]
        [TestCase("_f", TestName = "Should not find token without finishing token")]
        public void FindSimpleUnderscore(string paragraph)
        {
            var simpleUnderscore = new MarkdownToken("simpleUnderscore", "_", "em");

            var tokenFinder = new TokenFinder();
            var tokensWithPositions = tokenFinder.GetTokensWithPositions(paragraph);

            tokensWithPositions.Should().NotContainKey(simpleUnderscore);
        }

        [TestCase("_ff_", new[] { 0, 3 }, TestName = "Should find simple token")]
        [TestCase("_f _f_ _f_ f_", new[] { 0, 12, 3, 5, 7, 9 }, TestName = "Should find multiple token on one nesting level")]
        [TestCase("_f _f _f_ f_ f_", new[] { 0, 14, 3, 11, 6, 8 }, TestName = "Should find multiple nesting")]
        public void FindSimpleUnderscore(string paragraph, int[] positions)
        {
            var listOfExpectedPositions = new List<TokenPosition>();
            for (var i = 0; i < positions.Length; i += 2)
                listOfExpectedPositions.Add(new TokenPosition(positions[i], positions[i + 1]));
            
            var tokenFinder = new TokenFinder();
            var tokensWithPositions = tokenFinder.GetTokensWithPositions(paragraph);

            tokensWithPositions.First(token => token.Key.Name == "simpleUnderscore").Value.ShouldBeEquivalentTo(listOfExpectedPositions);
        }
    }
}