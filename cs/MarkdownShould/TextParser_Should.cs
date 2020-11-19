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
                new PlainTokenReader()
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
                new EmphasizedTextToken("_ab_") {SubTokens = new List<IToken> {new PlaintTextToken("ab")}}
            };
            var text = "_ab_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_NoClosingUnderlining()
        {
            var text = "_ab";
            var expectedList = new List<IToken> {new PlaintTextToken("_ab")};

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithThreeEmphasizedToken_TwoUnderliningElements()
        {
            var expectedList = new List<IToken>
            {
                new EmphasizedTextToken("_ab_") {SubTokens = new List<IToken> {new PlaintTextToken("ab")}},
                new PlaintTextToken(" "),
                new EmphasizedTextToken("_ba_") {SubTokens = new List<IToken> {new PlaintTextToken("ba")}}
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
                new PlaintTextToken("ab")
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
                new PlaintTextToken("aaa "),
                new EmphasizedTextToken("_bb_") {SubTokens = new List<IToken> {new PlaintTextToken("bb")}},
                new PlaintTextToken(" aaa "),
                new EmphasizedTextToken("_aa_") {SubTokens = new List<IToken> {new PlaintTextToken("aa")}}
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
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlaintTextToken("aa")}}
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
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlaintTextToken("aa")}},
                new PlaintTextToken(" "),
                new StrongTextToken("__bb__") {SubTokens = new List<IToken> {new PlaintTextToken("bb")}}
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
                new StrongTextToken("__aa__") {SubTokens = new List<IToken> {new PlaintTextToken("aa")}},
                new PlaintTextToken(" "),
                new EmphasizedTextToken("_bb_") {SubTokens = new List<IToken> {new PlaintTextToken("bb")}},
                new PlaintTextToken(" ac_")
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
                new PlaintTextToken("\\_ab\\_")
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
                new StrongTextToken("__\\_ab__") {SubTokens = new List<IToken> {new PlaintTextToken("\\_ab")}}
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
                new PlaintTextToken("____")
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
                new PlaintTextToken("a"),
                new PlaintTextToken("_a b_b")
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
                new PlaintTextToken("a"),
                new EmphasizedTextToken("_bc_") {SubTokens = new List<IToken> {new PlaintTextToken("bc")}},
                new PlaintTextToken("de")
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
                new PlaintTextToken("ab1_2_3")
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
                new HeaderTextToken("#ab") {SubTokens = new List<IToken> {new PlaintTextToken("ab")}}
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}