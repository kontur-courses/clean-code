using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class TokenParser_Tests
    {
        private readonly TokenParser tokenParser = new();

        [Test]
        public void Parse_Throw_WhenNullArgument()
        {
            Action act = () => tokenParser.Parse(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Parse_ReturnsNothing_WhenEmptyString()
        {
            tokenParser.Parse("")
                .Count()
                .Should()
                .Be(0);
        }

        [TestCase("abc", TestName = "one word without tags")]
        [TestCase("a b c", TestName = "string with spaces")]
        [TestCase("abc\n", TestName = "string with newline symbol")]
        public void Parse_ReturnsSingleTokenWithText_WhenNoTags(string text)
        {
            var expected = new PlainTextToken(text, 0, 0);
            AssertSingleToken(text, expected);
        }

        [Test]
        public void Parse_ReturnsSeveralPlainTextTokens_WhenSeparatedByLines()
        {
            var text = "abc\ndef";
            var expected = new List<Token>()
            {
                new PlainTextToken("abc\n", 0, 0),
                new PlainTextToken("def", 1, 0)
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        [TestCase("_abc_", TestName = "one word with italic tags")]
        [TestCase("_a b c_", TestName = "string with spaces (with tags)")]
        public void Parse_ReturnsItalicToken(string text)
        {
            var expected = new ItalicToken(text, 0, 0);
            AssertSingleToken(text, expected);
        }

        [TestCase("_abc", TestName = "no closing tag")]
        [TestCase("_abc _", TestName = "closing tag after space symbol")]
        [TestCase("_ abc_", TestName = "opening tag before space symbol")]
        [TestCase("_12_3", TestName = "tags inside number")]
        [TestCase("__", TestName = "empty string between tags")]
        [TestCase("a_bc cd_e", TestName = "selection in part of different words")]
        [TestCase("_abc __cd de_ fe__", TestName = "double tags intersection")]
        [TestCase("_ab__cd__e_", TestName = "bold inside italic tags")]
        public void Parse_ReturnSingleUnformattedToken_WhenTagUsingIsIncorrect(string text)
        {
            var expected = new PlainTextToken(text, 0, 0);
            AssertSingleToken(text, expected);
        }

        [Test]
        public void Parse_ReturnsCorrectTokens_WhenItalicAndPlainText()
        {
            var text = "_abc_ de";
            var expected = new List<Token>()
            {
                new ItalicToken("_abc_", 0, 0),
                new PlainTextToken(" de", 0, 5),
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_ReturnsItalicToken_WhenSelectionInsideWord()
        {
            var text = "_abc_de";
            var expected = new List<Token>()
            {
                new ItalicToken("_abc_", 0, 0),
                new PlainTextToken("de", 0, 5),
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        [TestCase("__abc__")]
        public void Parse_ReturnsBoldToken(string text)
        {
            var expected = new BoldToken(text, 0, 0);
            AssertSingleToken(text, expected);
        }

        [Test]
        public void Parse_ReturnsBoldAndItalic_WhenItalicTagsInsideBold()
        {
            var text = "__ab_cd_e__";
            var expected = new List<Token>()
            {
                new BoldToken("__ab_cd_e__", 0, 0),
                new PlainTextToken("_cd_", 0, 4),
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_ReturnsHeaderTag()
        {
            var text = "### abc";
            var expected = new HeaderToken(text, 0, 0);
            AssertSingleToken(text, expected);
        }

        [Test]
        public void Parse_ReturnsSeveralHeaderTag()
        {
            var text = "# abc\n# cde";
            var expected = new List<Token>()
            {
                new HeaderToken("# abc\n", 0, 0),
                new HeaderToken("# cde", 1, 0),
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Parse_IdentifyNestedTags_WhenTagsInsideHeader()
        {
            var text = "# abc _cde_ __fg__";
            var expected = new List<Token>()
            {
                new HeaderToken("# abc _cde_ __fg__", 0, 0),
                new ItalicToken("_cde_", 0, 6),
                new BoldToken("__fg__", 0, 12),
            };
            tokenParser.Parse(text).Should().BeEquivalentTo(expected);
        }

        private void AssertSingleToken(string text, Token expectedToken)
        {
            var actual = tokenParser.Parse(text).ToArray();
            actual.Single()
                .Should()
                .BeEquivalentTo(expectedToken);
        }
    }
}