using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class RulesTests
    {
        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1},
            new TokenInformation {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1}
        };


        [Test]
        public void DoubleUnderscoreBetweenUnderscoreRule_CorrectWork()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var tokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.Start, 8),
                new Token(dataForGraveAccent, TokenType.Start, 1),
                new Token(dataForGraveAccent, TokenType.End, 19),
                new Token(dataForUnderscore, TokenType.Start, 3),
                new Token(dataForUnderscore, TokenType.End, 15),
                new Token(dataForDouUnderscore, TokenType.End, 12)
            };
            var rule = new DoubleUnderscoreBetweenUnderscoreRule();
            var actualTokens = rule.Apply(tokens, baseTokens);

            var expectedTokens = new List<Token>
            {
                new Token(dataForGraveAccent, TokenType.Start, 1),
                new Token(dataForGraveAccent, TokenType.End, 19),
                new Token(dataForUnderscore, TokenType.Start, 3),
                new Token(dataForUnderscore, TokenType.End, 15)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void UnpairedSymbolsRule_CorrectList()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var tokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.End, 45),
                new Token(dataForDouUnderscore, TokenType.Start, 41),
                new Token(dataForDouUnderscore, TokenType.Start, 55),
                new Token(dataForDouUnderscore, TokenType.End, 49),
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForGraveAccent, TokenType.Start, 33),
                new Token(dataForGraveAccent, TokenType.End, 38),
                new Token(dataForGraveAccent, TokenType.End, 60),
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.Start, 20),
                new Token(dataForUnderscore, TokenType.End, 28)
            };
            var rule = new UnpairedSymbolsRule();
            var actualTokens = rule.Apply(tokens, baseTokens);

            var expectedTokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.Start, 41),
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForGraveAccent, TokenType.Start, 33),
                new Token(dataForGraveAccent, TokenType.End, 38),
                new Token(dataForGraveAccent, TokenType.End, 60),
                new Token(dataForUnderscore, TokenType.Start, 20),
                new Token(dataForUnderscore, TokenType.End, 28),
                new Token(dataForDouUnderscore, TokenType.End, 45)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void UnpairedSymbolsRule_OneTypeSymbols_CorrectList()
        {
            var underscore = baseTokens.First(x => x.Symbol == "_");
            var tokens = new List<Token>
            {
                new Token(underscore, TokenType.Start, 1),
                new Token(underscore, TokenType.Start, 3),
                new Token(underscore, TokenType.Start, 15),
                new Token(underscore, TokenType.End, 8),
                new Token(underscore, TokenType.End, 11),
                new Token(underscore, TokenType.End, 19),
                new Token(underscore, TokenType.End, 23)
            };
            var rule = new UnpairedSymbolsRule();
            var actualTokens = rule.Apply(tokens, baseTokens);

            var expectedTokens = new List<Token>
            {
                new Token(underscore, TokenType.Start, 1),
                new Token(underscore, TokenType.Start, 3),
                new Token(underscore, TokenType.End, 11),
                new Token(underscore, TokenType.Start, 15),
                new Token(underscore, TokenType.End, 8),
                new Token(underscore, TokenType.End, 19)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}