using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1},
            new TokenInformation {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1}
        };

        [Test]
        public void GetToken_SymbolsBetweenNumbers_EmptyList()
        {
            var str = "_89 5__95__ `565`8613";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, baseTokens);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_Backslash_ListWithEscapedSymbols()
        {
            var str = "\\__ jxkvnklnr";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, baseTokens);
            var expectedList = new List<Token>
            {
                new Token(baseTokens.First(x => x.Symbol == "\\"), TokenType.Escaped, 0),
                new Token(baseTokens.First(x => x.Symbol == "__"), TokenType.Ordinary, 1)
            };
            list.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTokens_DifferentSpecialSymbols_CorrectTokenList()
        {
            var mdText = "__la `topolya` and _puh_ i__";
            var parser = new TokenParser();
            var actualTokens = parser.GetTokens(mdText, baseTokens);
            var dataForDouUnderscore = baseTokens.First(x => x.Symbol == "__");
            var dataForUnderscore = baseTokens.First(x => x.Symbol == "_");
            var dataForGraveAccent = baseTokens.First(x => x.Symbol == "`");
            var expectedTokens = new List<Token>
            {
                new Token(dataForDouUnderscore, TokenType.Start, 0),
                new Token(dataForGraveAccent, TokenType.Start, 5),
                new Token(dataForGraveAccent, TokenType.End, 13),
                new Token(dataForUnderscore, TokenType.Start, 19),
                new Token(dataForUnderscore, TokenType.End, 23),
                new Token(dataForDouUnderscore, TokenType.End, 26)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_OnePairOfSpecialSymbols_CorrectTokenList()
        {
            var mdText = "__la__";
            var parser = new TokenParser();
            var actualTokens = parser.GetTokens(mdText, baseTokens);
            var doubleUnderscoreData = baseTokens.First(x => x.Symbol == "__");
            var expectedTokens = new List<Token>
            {
                new Token(doubleUnderscoreData, TokenType.Start, 0),
                new Token(doubleUnderscoreData, TokenType.End, 4)
            };
            actualTokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_OrdinaryText_EmptyTokenList()
        {
            var str = "hello its me";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, baseTokens);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_TextWithNoSpecialSymbols_EmptyTokenList()
        {
            var str = " ~ % @ hell!**o i+ts =me)()";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, baseTokens);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_WrongWhiteSpacePosition_EmptyList()
        {
            var str = "__ hello __";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, baseTokens);
            list.Should().BeEmpty();
        }
    }
}