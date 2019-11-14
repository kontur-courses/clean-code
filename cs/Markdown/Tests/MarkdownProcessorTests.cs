using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownProcessorTests
    {
        private MarkdownProcessor processor;

        [SetUp]
        public void SetUp()
        {
            processor = new MarkdownProcessor();
        }


        [Test]
        public void RenderToHtml_ShouldReturnUnchanged_WhenTextDoesNotContainMarkdownSymbols()
        {
            var text = "sample text";

            var renderedToHtmlText = processor.RenderToHtml(text);

            renderedToHtmlText.ShouldBeEquivalentTo(text);
        }

        [Test]
        public void RenderToHtml_ShouldReturnCorrect_WhenUnderscoreInText()
        {
            var text = "_sample text_";

            var renderedToHtmlText = processor.RenderToHtml(text);

            renderedToHtmlText.ShouldBeEquivalentTo("<em>sample text</em>");
        }
    }
}