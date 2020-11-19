using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.TokenReaders;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    internal class TextParser_Should
    {
        private TextParser textParser;

        [SetUp]
        public void SetUp()
        {
            var tokenReaders = new ITokenReader[]
            {
                new HeaderTokenReader(),
                new StrongTokenReader(),
                new EmphasizedTokenReader(),
                new PlainTextTokenReader()
            };
            textParser = new TextParser(tokenReaders);
        }

        [Test]
        public void GetTextTokens_ThrowArgumentException_NullText()
        {
            Action act = () => textParser.GetTextTokens(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GetTextTokens_ReturnEmptyList_EmptyText()
        {
            var textTokens = textParser.GetTextTokens("");

            textTokens.Should().BeEmpty();
        }

        [Test]
        public void GetTextTokens_ReturnListWithEmphasizedToken_OneUnderliningElement()
        {
            var expectedList = new List<IToken>
            {
                new EmphasizedTextToken("_ab_") {SubTokens = new List<IToken> {new PlainTextToken("ab")}}
            };
            var text = "_ab_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_NoClosingUnderlining()
        {
            var text = "_ab";
            var expectedList = new List<IToken> {new PlainTextToken("_ab")};

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithThreeEmphasizedToken_TwoUnderliningElements()
        {
            var expectedList = new List<IToken>
            {
                new EmphasizedTextToken("_ab_") {SubTokens = new List<IToken> {new PlainTextToken("ab")}},
                new PlainTextToken(" "),
                new EmphasizedTextToken("_ba_") {SubTokens = new List<IToken> {new PlainTextToken("ba")}}
            };
            var text = "_ab_ _ba_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_TextWithoutAnySpecialSymbols()
        {
            var text = "ab";
            var expectedList = new List<IToken>
            {
                new PlainTextToken("ab")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void
            GetTextTokens_ReturnListWithCorrectTokens_TextWithTwoUnderliningElementsAndTwoElementsWithoutAnySpecialSymbols()
        {
            var text = "aaa _bb_ aaa _aa_";
            var expectedList = new List<TextToken>
            {
                new PlainTextToken("aaa "),
                new EmphasizedTextToken("_bb_") {SubTokens = new List<IToken> {new PlainTextToken("bb")}},
                new PlainTextToken(" aaa "),
                new EmphasizedTextToken("_aa_") {SubTokens = new List<IToken> {new PlainTextToken("aa")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithOneDoubleUnderliningElement()
        {
            var text = "__aa__";
            var expectedList = new List<IToken>
            {
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlainTextToken("aa")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithTwoDoubleUnderliningElement()
        {
            var text = "__aa__ __bb__";
            var expectedList = new List<IToken>
            {
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlainTextToken("aa")}},
                new PlainTextToken(" "),
                new StrongTextToken("__bb__") {SubTokens = new List<IToken> {new PlainTextToken("bb")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithStrongEmphasizedAndNotClosedElements()
        {
            var text = "__aa__ _bb_ ac_";
            var expectedList = new List<IToken>
            {
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlainTextToken("aa")}},
                new PlainTextToken(" "),
                new EmphasizedTextToken("_bb_") {SubTokens = new List<IToken> {new PlainTextToken("bb")}},
                new PlainTextToken(" ac_")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldSymbol()
        {
            var text = "\\_ab\\_";
            var expectedList = new List<TextToken>
            {
                new PlainTextToken("\\_ab\\_")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldInsideStrongTag()
        {
            var text = "__\\_ab__";
            var expectedList = new List<IToken>
            {
                new StrongTextToken("__\\_ab__") {SubTokens = new List<IToken> {new PlainTextToken("\\_ab")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_NoTextBetweenUnderlinings()
        {
            var text = "____";
            var expectedList = new List<IToken>
            {
                new PlainTextToken("____")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithSpaceBetweenWords()
        {
            var text = "a_a b_b";
            var expectedList = new List<IToken>
            {
                new PlainTextToken("a"),
                new PlainTextToken("_a b_b")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithUnderliningsInsideWords()
        {
            var text = "a_bc_de";
            var expectedList = new List<IToken>
            {
                new PlainTextToken("a"),
                new EmphasizedTextToken("_bc_") {SubTokens = new List<IToken> {new PlainTextToken("bc")}},
                new PlainTextToken("de")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithNumbersBetweenUnderlinings()
        {
            var text = "ab1_2_3";
            var expectedList = new List<IToken>
            {
                new PlainTextToken("ab1_2_3")
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithHeader()
        {
            var text = "#ab";
            var expectedList = new List<IToken>
            {
                new HeaderTextToken("#ab") {SubTokens = new List<IToken> {new PlainTextToken("ab")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}