using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        [TestCase("_e_", "<em>e</em>", TestName = "One tag")]
        [TestCase("_e_ __S__", "<em>e</em> <strong>S</strong>", TestName = "Two tags")]
        [TestCase("# heading", "<h1> heading</h1>", TestName = "Heading")]
        [TestCase("__s _e_ _E_ s__", "<strong>s <em>e</em> <em>E</em> s</strong>", TestName = "Emphasized in strong")]
        [TestCase("# h _e_", "<h1> h <em>e</em></h1>", TestName = "Emphasized in heading")]
        [TestCase("# h _e_", "<h1> h <em>e</em></h1>", TestName = "Emphasized in heading")]
        [TestCase("Oleg Mongol", "Oleg Mongol", TestName = "Plain text")]
        [TestCase("_e __s__ e_", "<em>e __s__ e</em>", TestName = "Strong in emphasized")]
        [TestCase(@"te\xt", @"te\xt", TestName = "Backslash is not escaped")]
        [TestCase(@"\_e\_", @"_e_", TestName = "Backslash is escaped")]
        public void Render_ReturnExpectedResult_When(string text, string expectedResult)
        {
            var markdown = new Markdown.Markdown();
            var parser = new TextParser();
            var converter = new HtmlConverter();
            var htmlText = markdown.Render(text, parser, converter);

            htmlText.Should().Be(expectedResult);
        }
    }
}