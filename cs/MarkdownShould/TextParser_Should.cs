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
        public void GetTextTokens_ThrowArgumentException_NoClosingUnderlining()
        {
            var textParser = new TextParser();

            Action act = () => textParser.GetTextTokens("_ab");

            act.Should().Throw<ArgumentException>().WithMessage("No closing underlining");
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
    }
}
