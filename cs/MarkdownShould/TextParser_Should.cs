using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    internal class TextParser_Should
    {
        private TextParser textParser;

        [SetUp]
        public void SetUp()
        {
            var tokenGetters = new ITokenReader[]
            {
                new HeaderTokenReader(),
                new StrongTokenReader(),
                new EmphasizedTokenReader(),
                new TextTokenReader()
            };
            textParser = new TextParser(tokenGetters);
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
            var subTokens = new List<TextToken> {new TextToken(2, TokenType.Text, "ab", true)};
            var expectedList = new List<TextToken> {new TextToken(4, TokenType.Emphasized, "ab", subTokens)};
            var text = "_ab_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_NoClosingUnderlining()
        {
            var text = "_ab";
            var expectedList = new List<TextToken> {new TextToken(3, TokenType.Text, "_ab", true)};

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithThreeEmphasizedToken_TwoUnderliningElements()
        {
            var expectedList = new List<TextToken>
            {
                new TextToken(4, TokenType.Emphasized, "ab",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "ab", true)}),
                new TextToken(1, TokenType.Text, " ", true),
                new TextToken(4, TokenType.Emphasized, "ba",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "ba", true)})
            };
            var text = "_ab_ _ba_";

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithTextToken_TextWithoutAnySpecialSymbols()
        {
            var text = "ab";
            var expectedList = new List<TextToken>
            {
                new TextToken(2, TokenType.Text, "ab", true)
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
                new TextToken(4, TokenType.Text, "aaa ", true),
                new TextToken(4, TokenType.Emphasized, "bb",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "bb", true)}),
                new TextToken(5, TokenType.Text, " aaa ", true),
                new TextToken(4, TokenType.Emphasized, "aa",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "aa", true)})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithOneDoubleUnderliningElement()
        {
            var text = "__aa__";
            var expectedList = new List<TextToken>
            {
                new TextToken(6, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "aa", true)})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithStrongToken_TextWithTWoDoubleUnderliningElement()
        {
            var text = "__aa__ __bb__";
            var expectedList = new List<TextToken>
            {
                new TextToken(6, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "aa", true)}),
                new TextToken(1, TokenType.Text, " ", true),
                new TextToken(6, TokenType.Strong, "bb",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "bb", true)})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithStrongEmphasizedAndNotClosedElements()
        {
            var text = "__aa__ _bb_ ac_";
            var expectedList = new List<TextToken>
            {
                new TextToken(6, TokenType.Strong, "aa",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "aa", true)}),
                new TextToken(1, TokenType.Text, " ", true),
                new TextToken(4, TokenType.Emphasized, "bb",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "bb", true)}),
                new TextToken(4, TokenType.Text, " ac_", true)
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
                new TextToken(6, TokenType.Text, "_ab_", true)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithShieldInsideStrongTag()
        {
            var text = "__\\_ab__";
            var expectedList = new List<TextToken>
            {
                new TextToken(8, TokenType.Strong, "\\_ab",
                    new List<TextToken> {new TextToken(4, TokenType.Text, "_ab", true)})
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_NoTextBetweenUnderlinings()
        {
            var text = "____";
            var expectedList = new List<TextToken>
            {
                new TextToken(4, TokenType.Text, "____", true)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithSpaceBetweenWords()
        {
            var text = "a_a b_b";
            var expectedList = new List<TextToken>
            {
                new TextToken(1, TokenType.Text, "a", true),
                new TextToken(6, TokenType.Text, "_a b_b", true)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithUnderliningsInsideWords()
        {
            var text = "a_bc_de";
            var expectedList = new List<TextToken>
            {
                new TextToken(1, TokenType.Text, "a", true),
                new TextToken(4, TokenType.Emphasized, "bc",
                    new List<TextToken> {new TextToken(2, TokenType.Text, "bc", true)}),
                new TextToken(2, TokenType.Text, "de", true)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithNumbersBetweenUnderlinings()
        {
            var text = "ab1_2_3";
            var expectedList = new List<TextToken>
            {
                new TextToken(7, TokenType.Text, "ab1_2_3", true)
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }

        [Test]
        public void GetTextTokens_ReturnListWithCorrectTokens_TextWithHeader()
        {
            var text = "#ab";
            var expectedList = new List<TextToken>
            {
                new TextToken(3, TokenType.Header, "ab",
                    new List<TextToken>
                    {
                        new TextToken(2, TokenType.Text, "ab", true)
                    })
            };

            var actualList = textParser.GetTextTokens(text);

            actualList.Should().BeEquivalentTo(expectedList);
        }
    }
}