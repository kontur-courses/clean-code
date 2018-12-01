using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class EscapedSymbolsBetweenGraveAccentTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new EscapedSymbolsBetweenGraveAccent();
        }

        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, EndIsNewLine = true}
        };

        private EscapedSymbolsBetweenGraveAccent sut;

        [Test]
        public void Apply_NoGraveAccent_ExactText()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var tokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.End, 28),
                new Token(dataForDouUnderscore, TokenType.Start, 45),
                new Token(dataForDouUnderscore, TokenType.End, 49)
            };

            var actualTokens = sut.Apply(tokens);

            actualTokens.Should().BeEquivalentTo(tokens);
        }

        [Test]
        public void Apply_SymbolsBetweenGraveAccentEscaped()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var tokens = new List<Token>
            {
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.End, 28),
                new Token(dataForDouUnderscore, TokenType.Start, 45),
                new Token(dataForDouUnderscore, TokenType.End, 49),
                new Token(dataForGraveAccent, TokenType.End, 60)
            };

            var actualTokens = sut.Apply(tokens);

            var expectedTokens = new List<Token>
            {
                new Token(dataForGraveAccent, TokenType.Start, 0),
                new Token(dataForUnderscore, TokenType.Ordinary, 5),
                new Token(dataForUnderscore, TokenType.Ordinary, 28),
                new Token(dataForDouUnderscore, TokenType.Ordinary, 45),
                new Token(dataForDouUnderscore, TokenType.Ordinary, 49),
                new Token(dataForGraveAccent, TokenType.End, 60)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}