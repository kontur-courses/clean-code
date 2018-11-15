using System.Collections.Generic;
using NUnit.Framework;
using Markdown;
using Markdown.Elements;

namespace Markdown_Tests
{
    [TestFixture]
    class HtmlRenderer_Should
    {
        [Test]
        public void ReturnSameString_WhenOnlyRoot()
        {
            var content = "content";
            var markdownRoot = new MarkdownElement(RootElementType.Create(), content, 0,
                content.Length, new List<MarkdownElement>());
            
            var resultHtml = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(content, resultHtml);
        }

        [Test]
        public void ReturnWrappedToEm_WhenUnderscoreElement()
        {
            var content = "_content_";
            var markdownRoot = new MarkdownElement(UnderscoreElementType.Create(), content, 
                1, content.Length - 1, new List<MarkdownElement>());
            var expected = "<em>content</em>";

            var resultHtml = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(expected, resultHtml);
        }

        [Test]
        public void ReturnWrappedToStrong_WhenDoubleUnderscoreElement()
        {
            var content = "__content__";
            var markdownRoot = new MarkdownElement(DoubleUnderscoreElementType.Create(), content,
                2, content.Length - 2, new List<MarkdownElement>());
            var expected = "<strong>content</strong>";

            var resultHtml = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(expected, resultHtml);
        }

        [Test]
        public void ReturnRightMarkup_WhenRootHasOneInnerElement()
        {
            var content = "hello _world_";
            var markdownRoot = new MarkdownElement(
                RootElementType.Create(), content, 0, content.Length, 
                new List<MarkdownElement>
                {
                    new MarkdownElement(
                        UnderscoreElementType.Create(), content, 7, 12,
                        new List<MarkdownElement>())
                });

            var expected = "hello <em>world</em>";
            var actual = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnRightMarkup_WhenRootHasSeveralInnerElements()
        {
            var content = "__hello__ _world_";
            var markdownRoot = new MarkdownElement(
                RootElementType.Create(), content, 0, content.Length,
                new List<MarkdownElement>
                {
                    new MarkdownElement(
                        DoubleUnderscoreElementType.Create(), content, 2, 7,
                        new List<MarkdownElement>()),
                    new MarkdownElement(
                        UnderscoreElementType.Create(), content, 11, 16,
                        new List<MarkdownElement>())
                });

            var expected = "<strong>hello</strong> <em>world</em>";
            var actual = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReturnRightMarkup_WhenDepthIsMoreThanOne()
        {
            var content = "__hello _world_.__";
            var markdownRoot = new MarkdownElement(
                RootElementType.Create(), content, 0, content.Length,
                new List<MarkdownElement>
                {
                    new MarkdownElement(
                        DoubleUnderscoreElementType.Create(), content, 2, content.Length - 2,
                        new List<MarkdownElement>
                        {
                            new MarkdownElement(
                                UnderscoreElementType.Create(), content, 9, 14,
                                new List<MarkdownElement>())
                        })
                });

            var expected = "<strong>hello <em>world</em>.</strong>";
            var actual = HtmlRenderer.RenderToHtml(markdownRoot);

            Assert.AreEqual(expected, actual);
        }
    }
}
