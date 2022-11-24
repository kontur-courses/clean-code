using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class StrongShould
    {
        [Test]
        public void RenderMarkdownString_WhenTagIsStrong()
        {
            var markdownString = "__text__";
            var result = Md.Render(markdownString);
            result.Should().Be("\\<strong>text\\</strong>");
        }

        [Test]
        public void NotRenderMarkdownString_WhenStrongTagIsShielded()
        {
            var markdownString = "__text\\__";
            var result = Md.Render(markdownString);
            result.Should().Be("__text__");
        }
    }
}
