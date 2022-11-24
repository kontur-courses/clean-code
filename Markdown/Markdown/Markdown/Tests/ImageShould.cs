using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ImageShould
    {
        [Test]
        public void RenderMarkdownString_WhenImageIsAdded()
        {
            var markdownString = "**image.jpg**";
            var result = Md.Render(markdownString);
            result.Should().Be("<img src=\"image.jpg\">");
        }

        [Test]
        public void RenderMarkdownString_WhenTextWithImage()
        {
            var markdownString = "# some _text_ for __check__ all tags __with _special_ image__ : **image.jpg** \n";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<h1>some \\<em>text\\</em> for \\<strong>check\\</strong> all tags \\<strong>with \\<em>special\\</em> image\\</strong> : <img src=\"image.jpg\"> \\</h1>");
        }
    }
}
