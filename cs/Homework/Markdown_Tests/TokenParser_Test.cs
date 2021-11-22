using System;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class TokenParser_Test
    {
        private readonly TokenParser tokenParser = new();

        [Test]
        public void Parse_Throw_WhenNullArgument()
        {
            Action act = () => tokenParser.Parse(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Parse_ReturnSingleUnformattedEmptyToken_WhenEmptyString()
        {
            var result = tokenParser.Parse("");
            var token = result.Single();
            token.Should().BeEquivalentTo(new UnformattedToken(
                TokenType.Unformatted,
                "",
                0,
                0));
        }

        [TestCase("abc", TestName = "one word")]
        [TestCase("a b c", TestName = "string with spaces")]
        [TestCase("abc\n", TestName = "string with newline symbol")]
        [TestCase("abc\nde", TestName = "string with two lines")]
        public void Parse_ReturnSingleTokenWithText_WhenNoTags(string text)
        {
            var expected = new UnformattedToken(TokenType.Unformatted, text, 0, text.Length - 1);
            AssertSingleToken(text, expected);
        }

        [TestCase("_abc_", TestName = "one word")]
        [TestCase("_a b c_", TestName = "string with spaces")]
        public void Parse_ReturnSingleItalicToken(string text)
        {
            var expected = new ItalicToken(TokenType.Italic, text, 0, text.Length);
            AssertSingleToken(text, expected);
        }

        [TestCase("_abc", TestName = "no closing tag")]
        [TestCase("_abc _", TestName = "closing tag after space symbol")]
        [TestCase("_ abc_", TestName = "opening tag before space symbol")]
        [TestCase("_12_3", TestName = "tags inside number")]
        [TestCase("__", TestName = "empty string between tags")]
        [TestCase("_ab\nbc_", TestName = "tags in different lines")]
        [TestCase("__ab _bc__ cd_", TestName = "italic and bold tags intersection")]
        public void Parse_ReturnSingleUnformattedToken_WhenTagUsingIsIncorrect(string text)
        {
            var expected = new UnformattedToken(TokenType.Unformatted, text, 0, text.Length - 1);
            AssertSingleToken(text, expected);
        }

        [TestCase("abc _cde_", ExpectedResult = 9)]
        public int Parse_ReturnTokens_WhichSummaryLengthIsSameAsText(string text)
        {
            return tokenParser.Parse(text)
                .Sum(token => token.Length);
        }

        private void AssertSingleToken(string text, Token expectedToken)
        {
            var actual = tokenParser.Parse(text);
            actual.Single()
                .Should()
                .BeEquivalentTo(expectedToken);
        }
    }
}