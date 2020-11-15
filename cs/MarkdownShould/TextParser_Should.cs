using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using NUnit.Framework;

namespace Markdown.Tests
{
    class TextParser_Should
    {
        private IReadOnlyCollection<ITokenGetter> TokenGetters { get; set; }

        [SetUp]
        public void SetUp()
        {
            TokenGetters = new ITokenGetter[]
            {
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };
        }

        //В будущем частично заменю Test на TestCase
        //TODO НЕ ЗАБЫТЬ: ОДИН ТЕСТ - ОДИН КОММИТ!
        [Test]
        public void GetTextTokens_ThrowArgumentException_NullText()
        {
            var textParser = new TextParser(TokenGetters);
            Action act = () => textParser.GetTextTokens(null);

            act.Should().Throw<NullReferenceException>().WithMessage("string was null");
        }

        [Test]
        public void GetTextTokens_ReturnEmptyList_EmptyText()
        {
            var textParser = new TextParser(TokenGetters);

            var textTokens = textParser.GetTextTokens("");

            textTokens.Should().BeEmpty();
        }

        [Test]
        public void GetTextTokens_ReturnListWithEmphasizedToken_OneUnderliningElement()
        {
            var textParser = new TextParser(TokenGetters);
            var subTokens = new List<TextToken> {new TextToken(0, 2, TokenType.Text, "ab")};
            var expectedList = new List<TextToken> {new TextToken(1, 2, TokenType.Emphasized, "ab", subTokens)};
            var text = "_ab_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_NoClosingUnderlining()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "_ab";
            var expectedList = new List<TextToken> {new TextToken(0, 3, TokenType.Text, "_ab")};

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithThreeEmphasizedToken_TwoUnderliningElements()
        {
            var textParser = new TextParser(TokenGetters);
            var expectedList = new List<TextToken>
            {
                new TextToken(1, 2, TokenType.Emphasized, "ab",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "ab")}),
                new TextToken(4, 1, TokenType.Text, " "),
                new TextToken(6, 2, TokenType.Emphasized, "ba",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "ba")})
            };
            var text = "_ab_ _ba_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_TextWithoutAnySpecialSymbols()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "ab";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 2, TokenType.Text, "ab")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void
            GetTextTokens_ReturnListWithCorrectTokens_TextWithTwoUnderliningElementsAndTwoElementsWithoutAnySpecialSymbols()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "aaa _bb_ aaa _aa_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 4, TokenType.Text, "aaa "),
                new TextToken(5, 2, TokenType.Emphasized, "bb",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "bb")}),
                new TextToken(8, 5, TokenType.Text, " aaa "),
                new TextToken(14, 2, TokenType.Emphasized, "aa",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "aa")})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithOneDoubleUnderliningElement()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "__aa__";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "aa")}),
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithTWoDoubleUnderliningElement()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "__aa__ __bb__";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "aa")}),
                new TextToken(6, 1, TokenType.Text, " "),
                new TextToken(9, 2, TokenType.Strong, "bb",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "bb")})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithStrongEmphasizedAndNotClosedElements()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "__aa__ _bb_ ac_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "aa")}),
                new TextToken(6, 1, TokenType.Text, " "),
                new TextToken(8, 2, TokenType.Emphasized, "bb",
                    new List<TextToken> {new TextToken(0, 2, TokenType.Text, "bb")}),
                new TextToken(11, 4, TokenType.Text, " ac_")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldSymbol()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "\\_ab\\_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 4, TokenType.Text, "_ab_")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldInsideStrongTag()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "__\\_ab__";
            var expectedList = new List<TextToken>()
            {
                new TextToken(3, 3, TokenType.Strong, "_ab",
                    new List<TextToken> {new TextToken(0, 3, TokenType.Text, "_ab")})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }
        
        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_NoTextBetweenUnderlinings()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "____";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 4, TokenType.Text, "____",null)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);   
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithSpaceBetweenWords()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "a_a b_b";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 7, TokenType.Text, "a_a b_b",null)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);   
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithUnderliningsInsideWords()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "a_bc_de";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 1, TokenType.Text, "a",null),
                new TextToken(2,2,TokenType.Emphasized,"bc", 
                    new List<TextToken>{new TextToken(0,2,TokenType.Text,"bc")}),
                new TextToken(5,2, TokenType.Text, "de", null)
                
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);   
        }

        [Test]
        public void GetTextToknes_ReturnListWithCorrectTokens_TextWithNumbersBetweenUnderlinings()
        {
            var textParser = new TextParser(TokenGetters);
            var text = "ab1_2_3";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 7, TokenType.Text, "ab1_2_3",null),
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);   
        }
    }
}