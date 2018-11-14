using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class TagParser_Should
    {
        private MarkdownParser parser;
        private List<ISpanElement> spanElements;

        [SetUp]
        public void SetUp()
        {
            spanElements = new List<ISpanElement>();
            spanElements.Add(new SingleUnderscore());
            spanElements.Add(new DoubleUnderscore());
        }

        [Test]
        public void SplitMarkdownOnTags_GetPlainText_ReturnWithoutChanges()
        {
            var markdown = "plain text";
            parser = new MarkdownParser(markdown, spanElements);
            var tags = parser.ParseMarkdownOnHtmlTags();
            tags.First().HasHtmlWrap().Should().BeFalse();
        }

        [Test]
        public void GetNextSpanElement_GetPlainText_NoSpanElementDetected()
        {
            var markdown = "plain text";
            parser = new MarkdownParser(markdown, spanElements);
            for (int i = 0; i < markdown.Length; i++)
                parser.stringReader.DetermineSpanElement().Should().BeNull();
        }

        [TestCase("_single underscore_", typeof(SingleUnderscore), TestName = "DetectSingleUnderscoreSpanElement")]
        [TestCase("__double underscore__", typeof(DoubleUnderscore), TestName = "DetectDoubleUnderscoreSpanElement")]
        public void GetNextSpanElement_GetFormattedText(string markdown, Type type)
        {
            parser = new MarkdownParser(markdown, spanElements);
            var span = parser.stringReader.DetermineSpanElement();
            span.Should().BeOfType(type);
        }

        [TestCase("_ whitespace after single underscore_",  TestName = "WhiteSpaceAfterSingleUnderscore")]
        [TestCase("__ whitespace after double underscore_", TestName = "WhiteSpaceAfterDoubleUnderscore")]
        public void GetNextSpanElement_NoSpanElementDecetected(string markdown)
        {
            parser = new MarkdownParser(markdown, spanElements);
            parser.ParseNextTag();
            parser.ParseNextTag().SpanElement.Should().BeNull();
        }

        [Test]
        public void GetTagClosingPosition_GetPlainText_RetunIndexBeforeClosingIndicator()
        {
            var markdown = "plain text before underscores_italic_";
            parser = new MarkdownParser(markdown, spanElements);
            parser.ParseNextTag().Content.Should().Be("plain text before underscores");
        }

        [Test]
        public void ParseNextTag_GetPlainText_ReturnWithoutFormatting()
        {
            var markdown = "__italic _hello world_ italic__";
            parser = new MarkdownParser(markdown, spanElements);
            parser.ParseNextTag().Content.Should().Be("italic _hello world_ italic");
        }


        [Test]
        public void ParseNextTag_EscapedUnderscore_DoNotParseTag()
        {
            var markdown = "some\\__italic_";
            parser = new MarkdownParser(markdown, spanElements);
            var tag = parser.stringReader.DetermineSpanElement();
            tag.Should().BeNull();
        }


    }

}
