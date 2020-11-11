using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using NUnit.Framework;

namespace Markdown.Tests
{
    class TextParser_Should
    {
        //TODO НЕ ЗАБЫТЬ: ОДИН ТЕСТ - ОДИН КОММИТ!
        [Test]
        public void GetTextTokens_ThrowArgumentException_NullText()
        {
            var textParser = new TextParser();
            Action act = () => textParser.GetTextTokens(null);

            act.Should().Throw<ArgumentException>().WithMessage("string was null");

        }

        [Test]
        public void GetTextTokens_ReturnEmptyList_EmptyText()
        {
            var textParser = new TextParser();

            var textTokens = textParser.GetTextTokens("");

            textTokens.Should().BeEmpty();

        }

        [Test]
        public void GetTextTokens_ReturnListWithEmphasizedToken_OneUnderliningElement()
        {
            var textParser = new TextParser();
            var expectedList = new List<TextToken> {new TextToken(1, 2, TokenType.Emphasized , "ab")};
            var text = "_ab_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_NoClosingUnderlining()
        {
            var textParser = new TextParser();
            var expectedList = new List<TextToken>();
            expectedList.Add(new TextToken(0, 3, TokenType.Text, "_ab"));

            var actualList =  textParser.GetTextTokens("_ab");

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithThreeEmphasizedToken_TwoUnderliningElements()
        {
            var textParser = new TextParser();
            var expectedList = new List<TextToken>
            {
                new TextToken(1, 2, TokenType.Emphasized, "ab"),
                new TextToken(4,1,TokenType.Text, " "),
                new TextToken(6, 2, TokenType.Emphasized, "ba")
            };
            var text = "_ab_ _ba_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_TextWithoutAnySpecialSymbols()
        {
            var textParser = new TextParser();
            var text = "ab";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0,2, TokenType.Text,"ab")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithTwoUnderliningElementsAndTwoElementsWithoutAnySpecialSymbols()
        {
            var textParser = new TextParser();
            var text = "aaa_bb_aaa_aa_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0, 3, TokenType.Text, "aaa"),
                new TextToken(4, 2, TokenType.Emphasized, "bb"),
                new TextToken(7, 3, TokenType.Text, "aaa"),
                new TextToken(11, 2, TokenType.Emphasized, "aa")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithOneDoubleUnderliningElement()
        {
            var textParser = new TextParser();
            var text = "__aa__";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa"),
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithTWoDoubleUnderliningElement()
        {
            var textParser = new TextParser();
            var text = "__aa__ __bb__";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa"),
                new TextToken(6,1,TokenType.Text, " "),
                new TextToken(9, 2, TokenType.Strong, "bb")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithStrongEmphasizedAndNotClosedElements()
        {
            var textParser = new TextParser();
            var text = "__aa__ _bb_ac_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(2, 2, TokenType.Strong, "aa"),
                new TextToken(6,1,TokenType.Text, " "),
                new TextToken(8, 2, TokenType.Emphasized, "bb"),
                new TextToken(11,2,TokenType.Text,"ac")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldSymbol()
        {
            var textParser = new TextParser();
            var text = "\\_ab\\_";
            var expectedList = new List<TextToken>()
            {
                new TextToken(0,4,TokenType.Text,"_ab_")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}
