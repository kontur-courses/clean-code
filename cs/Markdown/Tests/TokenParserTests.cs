using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new TokenParser(baseTokens);
        }

        private TokenParser sut;

        private readonly List<TokenInformation> baseTokens = new List<TokenInformation>
        {
            new TokenInformation
                {Symbol = "__", Tag = "strong", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "_", Tag = "em", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "\\", Tag = "\\", IsPaired = false, EndIsNewLine = false},
            new TokenInformation {Symbol = "`", Tag = "code", IsPaired = true, EndIsNewLine = false},
            new TokenInformation {Symbol = "#", Tag = "h1", IsPaired = true, EndIsNewLine = true}
        };


        [Test]
        public void GetToken_EmptyBaseTokens_NotThrowException()
        {
            var mdText = "_hello_ `my friend`";
            Action act = () => sut.GetTokens(mdText);
            act.Should().NotThrow();
        }

        [Test]
        public void GetToken_SharpSymbol_ListWithHeadSymbols()
        {
            var mdText = "# HEAD " + Environment.NewLine +
                         "text";
            var sharpInformation = baseTokens.First(x => x.Symbol == "#");
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            var expectedList = new List<Token>
            {
                new Token(sharpInformation, TokenType.Start, 0),
                new Token(sharpInformation, TokenType.End, 7)
            };
            list.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetToken_SharpSymbolWithoutEndLine_ListWithHeadSymbols()
        {
            var mdText = "# HEAD " +
                         "text";
            var sharpInformation = baseTokens.First(x => x.Symbol == "#");
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            var expectedList = new List<Token>
            {
                new Token(sharpInformation, TokenType.Start, 0),
                new Token(sharpInformation, TokenType.End, 10)
            };
            list.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetToken_SymbolsBetweenNumbers_EmptyList()
        {
            var mdText = "_89 5__95__ `565`8613";
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_Backslash_ListWithEscapedSymbols()
        {
            var mdText = "\\__ jxkvnklnr";
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
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
            var parser = new TokenParser(baseTokens);
            var actualTokens = parser.GetTokens(mdText);
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
            var parser = new TokenParser(baseTokens);
            var actualTokens = parser.GetTokens(mdText);
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
            var mdText = "hello its me";
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_TextWithNoSpecialSymbols_EmptyTokenList()
        {
            var mdText = " ~ % @ hell!**o i+ts =me)()";
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            list.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_WrongWhiteSpacePosition_EmptyList()
        {
            var mdText = "__ hello __";
            var parser = new TokenParser(baseTokens);
            var list = parser.GetTokens(mdText);
            list.Should().BeEmpty();
        }
    }
}