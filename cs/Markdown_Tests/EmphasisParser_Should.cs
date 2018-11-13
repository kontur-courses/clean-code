using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Parsers;
using Markdown.Elements;


namespace Markdown_Tests
{
    [TestFixture]
    public class EmphasisParser_Should
    {
        [Test]
        public void ReturnPlainRootElement_WhenNoInnerElements()
        {
            var markdown = "hello world";
            var expected = new MarkdownElement(
                RootElementType.Create(), markdown, 0, markdown.Length, new List<MarkdownElement>());

            var actual = EmphasisParser.ParseElement(markdown, 0, RootElementType.Create());
            actual.Element.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ReturnUnderscoredElement_WhenValidClosing()
        {
            var markdown = "_hello world_";
            var expected = new MarkdownElement(
                UnderscoreElementType.Create(), markdown, 1, markdown.Length - 1, new List<MarkdownElement>());

            var actual = EmphasisParser.ParseElement(markdown, 1, UnderscoreElementType.Create());
            actual.Element.Should().BeEquivalentTo(expected);
        }
    }
}
