using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class UnpairedSymbolsRuleTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new UnpairedSymbolsRule(baseTokens);
        }

        private UnpairedSymbolsRule sut;

        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, CountOfSpaces = 1, EndIsNewLine = true}
        };

        [Test]
        public void Apply_DifferentSymbolsType_CorrectList()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var tokens = new List<Token>
            {
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.Start, 20),
                new Token(dataForUnderscore, TokenType.End, 28),
                new Token(dataForGraveAccent, TokenType.Start, 33),
                new Token(dataForGraveAccent, TokenType.End, 38),
                new Token(dataForDouUnderscore, TokenType.Start, 41),
                new Token(dataForDouUnderscore, TokenType.End, 45),
                new Token(dataForDouUnderscore, TokenType.End, 49),
                new Token(dataForDouUnderscore, TokenType.Start, 55),
                new Token(dataForGraveAccent, TokenType.End, 60)
            };

            var actualTokens = sut.Apply(tokens);

            var expectedTokens = new List<Token>
            {
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForUnderscore, TokenType.Start, 20),
                new Token(dataForUnderscore, TokenType.End, 28),
                new Token(dataForGraveAccent, TokenType.Start, 33),
                new Token(dataForGraveAccent, TokenType.End, 38),
                new Token(dataForDouUnderscore, TokenType.Start, 41),
                new Token(dataForDouUnderscore, TokenType.End, 45),
                new Token(dataForGraveAccent, TokenType.End, 60)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void Apply_EmptyList()
        {
            var tokens = new List<Token>();
            var actualTokens = sut.Apply(tokens);
            actualTokens.Should().BeEmpty();
        }

        [Test]
        public void Apply_OneTypeSymbols_CorrectList()
        {
            var underscore = baseTokens.First(x => x.Symbol == "_");
            var tokens = new List<Token>
            {
                new Token(underscore, TokenType.Start, 1),
                new Token(underscore, TokenType.Start, 3),
                new Token(underscore, TokenType.End, 8),
                new Token(underscore, TokenType.End, 11),
                new Token(underscore, TokenType.Start, 15),
                new Token(underscore, TokenType.End, 19),
                new Token(underscore, TokenType.End, 23)
            };

            var actualTokens = sut.Apply(tokens);

            var expectedTokens = new List<Token>
            {
                new Token(underscore, TokenType.Start, 1),
                new Token(underscore, TokenType.Start, 3),
                new Token(underscore, TokenType.End, 8),
                new Token(underscore, TokenType.End, 11),
                new Token(underscore, TokenType.Start, 15),
                new Token(underscore, TokenType.End, 19)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}