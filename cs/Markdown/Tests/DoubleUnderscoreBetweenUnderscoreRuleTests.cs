using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class DoubleUnderscoreBetweenUnderscoreRuleTests
    {
        private DoubleUnderscoreBetweenUnderscoreRule sut;

        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = true}
        };

        [SetUp]
        public void SetUp()
        {
            sut = new DoubleUnderscoreBetweenUnderscoreRule();
        }

        [Test]
        public void Apply_EmptySymbolsMap_ReturnEmptyList()
        {
            var tokens = new List<Token>();
            var actualTokens = sut.Apply(tokens);
            actualTokens.Should().BeEmpty();
        }

        [Test]
        public void Apply_TwoDoubleUnderscoreBetweenUnderscore_DeleteDoubleUnderscore()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var mdTokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Start, 3),
                new Token(dataForDouUnderscore, TokenType.Start, 8),
                new Token(dataForUnderscore, TokenType.End, 15),
                new Token(dataForDouUnderscore, TokenType.End, 12)
            };

            var htmlTokens = sut.Apply(mdTokens);

            var expectedTokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Start, 3),
                new Token(dataForUnderscore, TokenType.End, 15)
            };
            htmlTokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}