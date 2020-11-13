using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HtmlConverterTests
    {
        [TestCase("__text__", TokenType.Strong, "<strong>text</strong>", TestName = "Strong tag")]
        [TestCase("_text_", TokenType.Emphasized, "<em>text</em>", TestName = "Emphasized tag")]
        [TestCase("# text", TokenType.Heading, "<h1> text</h1>", TestName = "Heading tag")]
        [TestCase("text", TokenType.PlainText, "text", TestName = "PlainText")]
        public void ConvertTokensToHtml_ReturnExpectedResult_When(
            string text, TokenType type, string expectedResult)
        {
            var htmlConverter = new HtmlConverter();
            var tokens = new List<Token> {new Token(0, text, type)};

            htmlConverter.ConvertTokensToHtml(tokens).Should().Be(expectedResult);
        }
    }
}