using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkDownTests
{
    [TestFixture]
    public class MarkDown_Tests
    {
        private MarkdownConverter converter = new MarkdownConverter();

        [Test]
        public void MarkdownConverter_ShouldConvertItalicTag()
        {
            var markDownText = "abc _def_ gh";
            var htmlText = converter.ConvertToHtml(markDownText);
            htmlText.Should().Be("abc <em>def</em> gh");
        }

        [Test]
        public void MarkdownConverter_ShouldConvertBoldTag()
        {
            var markDownText = "abc __def__ gh";
            var htmlText = converter.ConvertToHtml(markDownText);
            htmlText.Should().Be("abc <strong>def</strong> gh");
        }

        [Test]
        public void MarkdownConverter_ShouldConvertHeaderTag()
        {
            var markDownText = "#abcdefgh\n";
            var htmlText = converter.ConvertToHtml(markDownText);
            htmlText.Should().Be("<h1>abcdefgh</h1>\n");
        }
    }
}