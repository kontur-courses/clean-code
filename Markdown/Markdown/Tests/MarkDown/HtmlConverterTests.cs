using NUnit.Framework;
using FluentAssertions;
using Markdown.Parser;

namespace Markdown.Tests.MarkDown
{
    internal class HtmlConverterTests
    {
        private Markdown.MarkDown.MdProcessor mdProcessor;

        [SetUp]
        public void SetUp()
        {
            mdProcessor = new Markdown.MarkDown.MdProcessor(new MdTagParser());
        }


        [Test]
        public void Should_WrapIntoParagraphTag()
        {
            mdProcessor.Render("__abc__").
                Should()
                .Be("<p><strong>abc</strong></p>");
        }

        [Test]
        public void Should_WorkWithMoreThanOneTagInOneParagraph()
        {
            mdProcessor.Render("__abc ~de~ fg__ _hi\nj_")
                .Should()
                .Be("<p><strong>abc <strike>de</strike> fg</strong> <em>hi\nj</em></p>");
        }

        [Test]
        public void Should_WorkWithMoreThanOneParagraph()
        {
            mdProcessor.Render("###header__abc__\r\n\r\n__some text__")
                .Should()
                .Be("<p><h3>header__abc__</h3></p>\r\n\r\n<p><strong>some text</strong></p>");
        }
    }
}
