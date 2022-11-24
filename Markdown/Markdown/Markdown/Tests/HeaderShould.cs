using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HeaderShould
    {
        [Test]
        public void RenderMarkdownString_WhenTagIsHeader()
        {
            var markdownString = "# some text\n";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<h1>some text\\</h1>");
        }

        [Test]
        public void WhenHeaderTagIsShielded()
        {
            var markdownString = "\\# text \\\n";
            var result = Md.Render(markdownString);
            result.Should().Be("# text \n");
        }
    }
}
