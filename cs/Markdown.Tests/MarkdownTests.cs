using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.Readers;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownTests
    {
        private readonly List<ITokenReader> readers = new List<ITokenReader>
        {
            new HeadingTokenReader(),
            new StrongTokenReader(),
            new EmphasizedTokenReader(),
            new ImageTokenReader(),
            new PlainTextTokenReader()
        };

        private IConverter converter;
        private Markdown.Markdown markdown;

        private ITextParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(readers);
            converter = new HtmlConverter(new TokenConverterFactory());
            markdown = new Markdown.Markdown(parser, converter);
        }

        [TestCase("_e_", "<em>e</em>", TestName = "One tag")]
        [TestCase("_e_ __S__", "<em>e</em> <strong>S</strong>", TestName = "Two tags")]
        [TestCase("# heading", "<h1>heading</h1>", TestName = "Heading")]
        [TestCase("__s _e_ _E_ s__", "<strong>s <em>e</em> <em>E</em> s</strong>", TestName = "Emphasized in strong")]
        [TestCase("# h _e_", "<h1>h <em>e</em></h1>", TestName = "Emphasized in heading")]
        [TestCase("Oleg Mongol", "Oleg Mongol", TestName = "Plain text")]
        [TestCase("_e __s__ e_", "<em>e __s__ e</em>", TestName = "Strong in emphasized")]
        [TestCase(@"te\xt", @"te\xt", TestName = "Backslash is not escaped")]
        [TestCase(@"\_e\_", @"_e_", TestName = "Backslash is escaped")]
        [TestCase(@"_e\\_", @"<em>e\</em>", TestName = "Backslash before backslash")]
        [TestCase(@"\![]()", @"![]()", TestName = "Backslash before image")]
        [TestCase(@"![] text ()", @"![] text ()", TestName = "Nothing between image tags")]
        [TestCase(@"# h __s _E _e_ E_ s__ _e_", @"<h1>h <strong>s <em>E <em>e</em> E</em> s</strong> <em>e</em></h1>",
            TestName = "Deep nesting")]
        [TestCase(@"_e __s e_ s__", @"_e __s e_ s__", TestName = "Emphasized intersect strong")]
        [TestCase(@"__s _e s__ e_", @"__s _e s__ e_", TestName = "Strong intersect emphasized")]
        [TestCase("# 1\n# 2\n# 3", "<h1>1</h1>\n<h1>2</h1>\n<h1>3</h1>", TestName = "More than one heading")]
        [TestCase(@"__S E_ text", @"__S E_ text", TestName = "Not paired")]
        [TestCase(@"_e __e", @"_e __e", TestName = "No tag end")]
        [TestCase(@"s__ s_", @"s__ s_", TestName = "No start tag")]
        [TestCase("__s \n s__", "__s \n s__", TestName = "New line")]
        [TestCase("_e \r\n e_", "_e \r\n e_", TestName = "Non-unix new line")]
        [TestCase("![\\]()", "![\\]()", TestName = "Backslash before alt text end tag")]
        [TestCase("!\\[]()", "!\\[]()", TestName = "Backslash before alt text start tag")]
        [TestCase("![](\\)", "![](\\)", TestName = "Backslash before closing tag")]
        [TestCase(@"![cat](https://i.ibb.co/fxwGJZB/image.png)",
            @"<img src=""https://i.ibb.co/fxwGJZB/image.png"" alt=""cat"">",
            TestName = "Image")]
        [TestCase(@"____", @"____", TestName = "Empty tag")]
        [TestCase(@"o__ne tw__o", @"o__ne tw__o", TestName = "Different words")]
        [TestCase(@"_sta_rt", @"<em>sta</em>rt", TestName = "In word start")]
        [TestCase(@"mi__dd__le", @"mi<strong>dd</strong>le", TestName = "In word middle")]
        [TestCase(@"en_d._", @"en<em>d.</em>", TestName = "In word end")]
        public void Render_ReturnExpectedResult_When(string text, string expectedResult)
        {
            var htmlText = markdown.Render(text);

            htmlText.Should().Be(expectedResult);
        }
    }
}