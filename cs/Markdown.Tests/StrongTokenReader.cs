using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class StrongTokenReaderTests
    {
        [TestCase("__one__", 0, "__one__", TokenType.Strong, TestName = "One token")]
        [TestCase("__long text__", 0, "__long text__", TokenType.Strong, TestName = "More than one words in one tag")]
        [TestCase("__s _e_ s__", 0, "__s _e_ s__", TokenType.Strong, TestName = "Emphasized tag in strong")]
        [TestCase("__sta__rt", 0, "__sta__", TokenType.Strong, TestName = "In word start")]
        [TestCase("mi__dd__le", 2, "__dd__", TokenType.Strong, TestName = "In word middle")]
        [TestCase("en__d.__", 2, "__d.__", TokenType.Strong, TestName = "In word end")]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, string expectedValue, TokenType expectedType)
        {
            var reader = new StrongTokenReader();
            var result = reader.TryReadToken(text, position);

            result!.Value.Should().Be(expectedValue);
            result.Type.Should().Be(expectedType);
        }
    }
}