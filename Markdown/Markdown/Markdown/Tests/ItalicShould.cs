using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ItalicShould
    {
        [Test]
        public void RenderMarkdownString_WhenTagIsItalic()
        {
            var markdownString = "some _text_";
            var result = Md.Render(markdownString);
            result.Should().Be("some \\<em>text\\</em>");
        }

        [Test]
        public void NotRenderMarkdownString_WhenItalicTagIsShielded()
        {
            var markdownString = "some \\_text\\_";
            var result = Md.Render(markdownString);
            result.Should().Be("some _text_");
        }
    }
}
