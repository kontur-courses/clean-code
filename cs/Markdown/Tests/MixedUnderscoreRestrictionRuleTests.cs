using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MixedUnderscoreRestrictionRuleTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new MixedUnderscoreRestrictionRule();
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

        private MixedUnderscoreRestrictionRule sut;

        [Test]
        public void Apply_DoubleUnderscoreStartBetweenUnderscore()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var changedDataForLast = dataForDouUnderscore.WithTag("em");
            var tokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForDouUnderscore, TokenType.Start, 28),
                new Token(dataForUnderscore, TokenType.End, 45),
                new Token(dataForDouUnderscore, TokenType.End, 49)
            };
            var expectedTokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Start, 5),
                new Token(dataForDouUnderscore, TokenType.Escaped, 28),
                new Token(dataForUnderscore, TokenType.Escaped, 45),
                new Token(changedDataForLast, TokenType.End, 49)
            };
            var actualTokens = sut.Apply(tokens);

            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void Apply_NoUnderscores_ExactList()
        {
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "\\");
            var tokens = new List<Token>
            {
                new Token(dataForUnderscore, TokenType.Ordinary, 5),
                new Token(dataForGraveAccent, TokenType.Start, 28),
                new Token(dataForGraveAccent, TokenType.End, 49)
            };

            var actualTokens = sut.Apply(tokens);

            actualTokens.Should().BeEquivalentTo(tokens);
        }
        
        [Test]
        public void Apply_UnderscoreStartBetweenDoubleUnderscore()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var changedDataForFirst = dataForDouUnderscore.WithTag("em");
            var tokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.Start, 28),
                new Token(dataForDouUnderscore, TokenType.End, 45),
                new Token(dataForUnderscore, TokenType.End, 49)
            };
            var expectedTokens = new List<Token>
            {
                new Token(changedDataForFirst, TokenType.Start, 5),
                new Token(dataForUnderscore, TokenType.Escaped, 28),
                new Token(dataForDouUnderscore, TokenType.Escaped, 45),
                new Token(dataForUnderscore, TokenType.End, 49)
            };
            var actualTokens = sut.Apply(tokens);

            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }
        
        [Test]
        public void Apply_UnderscorePairAfterPairDoubleUnderscore_ExactTokens()
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
        public void Apply_DoubleUnderscorePairAfterPairUnderscore_ExactTokens()
        {
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var tokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.Start, 5),
                new Token(dataForDouUnderscore, TokenType.End, 28),
                new Token(dataForUnderscore, TokenType.Start, 45),
                new Token(dataForUnderscore, TokenType.End, 49)
            };

            var actualTokens = sut.Apply(tokens);

            actualTokens.Should().BeEquivalentTo(tokens);
        }
    }
}