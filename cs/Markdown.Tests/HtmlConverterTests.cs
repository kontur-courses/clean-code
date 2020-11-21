using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HtmlConverterTests
    {
        private ITextParser Parser { get; set; }
        private IConverter Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            Parser = new TextParser();
            Sut = new HtmlConverter();
        }

        [TestCase("__text__", TokenType.Strong, "<strong>text</strong>", TestName = "Strong tag")]
        [TestCase("_text_", TokenType.Emphasized, "<em>text</em>", TestName = "Emphasized tag")]
        [TestCase("# text", TokenType.Heading, "<h1> text</h1>", TestName = "Heading tag")]
        [TestCase("text", TokenType.PlainText, "text", TestName = "PlainText")]
        [TestCase("![alt text](url)", TokenType.Image, @"<img src=""url"" alt=""alt text"">", TestName = "Image")]
        [TestCase("![]()", TokenType.Image, @"<img src="""" alt="""">", TestName = "Empty image")]
        public void ConvertTokensToHtml_ReturnExpectedResult_When(
            string text, TokenType type, string expectedResult)
        {
            var tokens = Parser.GetTokens(text, text);

            Sut.ConvertTokens(tokens).Should().Be(expectedResult);
        }
    }
}