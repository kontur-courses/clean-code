using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ImageTokenReaderTests
    {
        [TestCase("![]()", 0, "![]()", TokenType.Image, TestName = "Empty image")]
        [TestCase("![alt text](url)", 0, "![alt text](url)", TokenType.Image, TestName = "Image with attributes")]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, string expectedValue, TokenType expectedType)
        {
            var reader = new ImageTokenReader();
            var result = reader.TryReadToken(text, position);

            result!.Value.Should().Be(expectedValue);
            result.Type.Should().Be(expectedType);
        }
    }
}