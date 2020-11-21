using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        private ITextParser Parser { get; set; }
        private IConverter Converter { get; set; }
        private Markdown.Markdown Sut { get; set; }

        [SetUp]
        public void SetUp()
        {
            Parser = new TextParser();
            Converter = new HtmlConverter();
            Sut = new Markdown.Markdown(Parser, Converter);
        }

        [TestCase("_e_", "<em>e</em>", TestName = "One tag")]
        [TestCase("_e_ __S__", "<em>e</em> <strong>S</strong>", TestName = "Two tags")]
        [TestCase("# heading", "<h1> heading</h1>", TestName = "Heading")]
        [TestCase("__s _e_ _E_ s__", "<strong>s <em>e</em> <em>E</em> s</strong>", TestName = "Emphasized in strong")]
        [TestCase("# h _e_", "<h1> h <em>e</em></h1>", TestName = "Emphasized in heading")]
        [TestCase("Oleg Mongol", "Oleg Mongol", TestName = "Plain text")]
        [TestCase("_e __s__ e_", "<em>e __s__ e</em>", TestName = "Strong in emphasized")]
        [TestCase(@"te\xt", @"te\xt", TestName = "Backslash is not escaped")]
        [TestCase(@"\_e\_", @"_e_", TestName = "Backslash is escaped")]
        [TestCase(@"_e\\_", @"<em>e\</em>", TestName = "Backslash before backslash")]
        [TestCase(@"\![]()", @"![]()", TestName = "Backslash before image")]
        [TestCase(@"![] text ()", @"![] text ()", TestName = "Nothing between image tags")]
        [TestCase(@"# h __s _E _e_ E_ s__ _e_", @"<h1> h <strong>s <em>E <em>e</em> E</em> s</strong> <em>e</em></h1>", TestName = "Deep nesting")]
        public void Render_ReturnExpectedResult_When(string text, string expectedResult)
        {
            var htmlText = Sut.Render(text);

            htmlText.Should().Be(expectedResult);
        }
    }
}