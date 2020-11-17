using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class EmphasizedTokenReaderTests
    {
        [TestCase("_word_", 0, "_word_", TokenType.Emphasized, TestName = "One token")]
        [TestCase("_long text_", 0, "_long text_", TokenType.Emphasized, TestName = "Two words in one tag")]
        [TestCase("_sta_rt", 0, "_sta_", TokenType.Emphasized, TestName = "In word start")]
        [TestCase("mi_dd_le", 2, "_dd_", TokenType.Emphasized, TestName = "In word middle")]
        [TestCase("en_d._", 2, "_d._", TokenType.Emphasized, TestName = "In word end")]
        [TestCase("_e __s__ e_", 0, "_e __s__ e_", TokenType.Emphasized, TestName = "Strong tag in emphasized")]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, string expectedValue, TokenType expectedType)
        {
            var reader = new EmphasizedTokenReader();
            var result = reader.TryReadToken(text, position);

            result!.Value.Should().Be(expectedValue);
            result.Type.Should().Be(expectedType);
        }
    }
}