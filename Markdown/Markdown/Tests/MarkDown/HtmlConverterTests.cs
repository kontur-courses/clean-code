using System;
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
            mdProcessor.Render($"__abc ~de~ fg__ _hi{Environment.NewLine}j_")
                .Should()
                .Be($"<p><strong>abc <strike>de</strike> fg</strong> <em>hi{Environment.NewLine}j</em></p>");
        }

        [Test]
        public void Should_WorkWithMoreThanOneParagraph()
        {
            mdProcessor.Render($"###header__abc__{Environment.NewLine + Environment.NewLine}__some text__")
                .Should()
                .Be($"<p><h3>header__abc__</h3></p>{Environment.NewLine + Environment.NewLine}<p><strong>some text</strong></p>");
        }
    }
}
