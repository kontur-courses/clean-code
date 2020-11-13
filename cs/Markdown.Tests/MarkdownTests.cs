using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        [TestCase("_e_", "<em>e</em>", TestName = "One tag")]
        [TestCase("_e_ __S__", "<em>e</em> <strong>S</strong>", TestName = "Two tags")]
        [TestCase("# heading", "<h1> heading</h1>", TestName = "Heading")]
        public void Render_ReturnExpectedResult_When(string text, string expectedResult)
        {
            var markdown = new Markdown.Markdown();
            var htmlText = markdown.Render(text);

            htmlText.Should().Be(expectedResult);
        }
    }
}