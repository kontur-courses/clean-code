using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        private List<TokenInformation> data = new List<TokenInformation>
        {
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, CountOfSpaces = 1},
            new TokenInformation {Symbol = "__", Tag = "strong", IsPaired = true, CountOfSpaces = 2},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, CountOfSpaces = 1},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, CountOfSpaces = 1}
        };

        [Test]
        public void GetTokens_DifferentSpecialSymbols_CorrectTokenList()
        {
            var mdText = "__la `topolya` and _puh_ i__";
            var parser = new TokenParser();
            var actualTokens = parser.GetTokens(mdText, data);
            var dataForDouUnderscore = data.First(x => x.Symbol == "__");
            var dataForUnderscore = data.First(x => x.Symbol == "_");
            var dataForGraveAccent = data.First(x => x.Symbol == "`");
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
            var actualTokens = parser.GetTokens(mdText, data);
            var doubleUnderscoreData = data.First(x => x.Symbol == "__");
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
            var list = parser.GetTokens(str, data);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_TextWithNoSpecialSymbols_EmptyTokenList()
        {
            var str = " ~ % @ hell!**o i+ts =me)()";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, data);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_Backslash_ListWithEscapedSymbols()
        {
            var str = "\\__ jxkvnklnr";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, data);
            var expectedList = new List<Token>
            {
                new Token(data.First(x => x.Symbol == "\\"), TokenType.Escaped, 0),
                new Token(data.First(x => x.Symbol == "__"), TokenType.Ordinary, 1)
            };
            list.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTokens_WrongWhiteSpacePosition_EmptyList()
        {
            var str = "__ hello __";
            var parser = new TokenParser();
            var list = parser.GetTokens(str, data);
            list.Should().BeEmpty();
        }

        [Test]
        public void DoSomething_WhenSomething()
        {
            
        }
    }
}