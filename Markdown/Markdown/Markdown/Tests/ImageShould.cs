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
            var markdownString = "image: ![AltText](image.jpg)";
            var result = Md.Render(markdownString);
            result.Should().Be("image: <img src=\"image.jpg\" alt=\"AltText\">");
        }
        [Test]
        public void NotRenderMarkdownString_WhenImageStartHaveWhiteSpace()
        {
            var markdownString = "image: ! [AltText](image.jpg)";
            var result = Md.Render(markdownString);
            result.Should().Be("image: ! [AltText](image.jpg)");
        }
        [Test]
        public void NotRenderMarkdownString_WhenImageDescriptionHaveWhitespace()
        {
            var markdownString = "image: ![AltText] (image.jpg)";
            var result = Md.Render(markdownString);
            result.Should().Be("image: ![AltText] (image.jpg)");
        }

        [Test]
        public void RenderMarkdownString_WhenTextHaveManyImages()
        {
            var markdownString = "![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)![AltText](image.jpg)";
            var result = Md.Render(markdownString);
            result.Should()
                .Be("<img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\"><img src=\"image.jpg\" alt=\"AltText\">");
        }


        [Test]
        public void RenderMarkdownString_WhenTextWithImage()
        {
            var markdownString = "# some _text_ for __check__ all tags __with _special_ image__ : ![AltText](image.jpg) \n";
            var result = Md.Render(markdownString);
            result.Should()
                .Be("\\<h1>some \\<em>text\\</em> for \\<strong>check\\</strong> all tags \\<strong>with \\<em>special\\</em> image\\</strong> : <img src=\"image.jpg\" alt=\"AltText\"> \\</h1>");
        }
    }
}
