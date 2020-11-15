using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenTests
    {
        [TestCase(TokenType.Emphasized, "_e_", "e", TestName = "Emphasized tag")]
        [TestCase(TokenType.Strong, "__s__", "s", TestName = "Strong tag")]
        [TestCase(TokenType.Heading, "# h", " h", TestName = "Heading tag")]
        [TestCase(TokenType.PlainText, "text", "text", TestName = "Plain text")]
        public void GetValueWithoutTags_ReturnExpectedValue_When(
            TokenType type, string value, string expectedValue)
        {
            var token = new Token(0, value, type);

            token.GetValueWithoutTags().Should().Be(expectedValue);
        }
    }
}