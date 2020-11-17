using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HeadingTokenReaderTests
    {
        [TestCase("# one", 0, "# one", TokenType.Heading, TestName = "One heading")]
        [TestCase("# one\n# two\n# three", 6, "# two", TokenType.Heading, TestName = "Heading in text")]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, string expectedValue, TokenType expectedType)
        {
            var reader = new HeadingTokenReader();
            var result = reader.TryReadToken(text, position);

            result!.Value.Should().Be(expectedValue);
            result.Type.Should().Be(expectedType);
        }
    }
}