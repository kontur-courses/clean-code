using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class InlineTokenFinderTests
    {
        private List<SingleToken> GetValidTokens(string paragraph)
        {
            var finder = new InlineTokenFinder();
            var validator = new InlineTokensValidator();
            var tokens = finder.FindInlineTokensInMdText(paragraph);
            var validTokens = validator.GetValidTokens(tokens);

            return validTokens;
        }

        [TestCase("__f _d_ f__", new[] { 4, 6 }, new[] { 0, 9 }, TestName = "Should find simple and double token")]
        [TestCase("__f _d__ f_", new int[0], new[] { 0, 6 }, TestName = "Should works correct on crossing of different tokens")]
        public void FindDoubleAndSimple(string paragraph, int[] simpleUnderscorePositions, int[] doubleUnderscorePositions)
        {
            var validTokens = GetValidTokens(paragraph);

            var positionsForSimpleUnderscore =
                validTokens.Where(token => token.TokenType.Name == "simpleUnderscore")
                    .Select(token => token.TokenPosition);
            var positionsForDoubleUnderscore =
                validTokens.Where(token => token.TokenType.Name == "doubleUnderscore")
                    .Select(token => token.TokenPosition);

            positionsForSimpleUnderscore.ShouldBeEquivalentTo(simpleUnderscorePositions);
            positionsForDoubleUnderscore.ShouldBeEquivalentTo(doubleUnderscorePositions);
        }

        [Test]
        public void ShouldFindDoubleUnderscore()
        {
            var validTokens = GetValidTokens("__f__");

            validTokens.Where(token => token.TokenType.Name == "doubleUnderscore")
                .Select(token => token.TokenPosition)
                .ShouldBeEquivalentTo(new[] { 0, 3 });
        }

        [TestCase("_ff\\_", TestName = "Should not find finishing token with screening")]
        [TestCase("\\_ff_", TestName = "Should not find starting token with screening")]
        [TestCase("f_", TestName = "Should not find token without starting token")]
        [TestCase("_f", TestName = "Should not find token without finishing token")]
        public void FindSimpleUnderscore(string paragraph)
        {
            var validTokens = GetValidTokens(paragraph);

            validTokens.Select(token => token.TokenType.Name).Should().NotContain("simpleUnderscore");
        }

        [TestCase("_ff_", new[] { 0, 3 }, TestName = "Should find simple token")]
        [TestCase("_f _f_ _f_ f_", new[] { 0, 12, 3, 5, 7, 9 }, TestName = "Should find multiple token on one nesting level")]
        [TestCase("_f _f _f_ f_ f_", new[] { 0, 14, 3, 11, 6, 8 }, TestName = "Should find multiple nesting")]
        public void FindSimpleUnderscore(string paragraph, int[] positions)
        {
            var validTokens = GetValidTokens(paragraph);

            validTokens.Where(token => token.TokenType.Name == "simpleUnderscore")
                .Select(token => token.TokenPosition)
                .ShouldBeEquivalentTo(positions);
        }
    }
}