using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class PlainTextTokenReaderTests
    {
        [TestCase("text", 0, "text", TokenType.PlainText, TestName = "Only plain text")]
        [TestCase("_e_ text", 3, " text", TokenType.PlainText, TestName = "Plain text at the end")]
        [TestCase("text __s__", 0, "text ", TokenType.PlainText, TestName = "Plain text at the start")]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, string expectedValue, TokenType expectedType)
        {
            var reader = new PlainTextTokenReader();
            var result = reader.TryReadToken(text, position);

            result!.Value.Should().Be(expectedValue);
            result.Type.Should().Be(expectedType);
        }
    }
}