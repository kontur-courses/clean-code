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
                parser.DetermineSpanElement().Should().BeNull();
        }

        [TestCase("_single underscore_", typeof(SingleUnderscore), TestName = "DetectSingleUnderscoreSpanElement")]
        [TestCase("__double underscore__", typeof(DoubleUnderscore), TestName = "DetectDoubleUnderscoreSpanElement")]
        public void GetNextSpanElement_GetFormattedText(string markdown, Type type)
        {
            parser = new MarkdownParser(markdown, spanElements);
            var span = parser.DetermineSpanElement();
            span.Should().BeOfType(type);
        }

        [TestCase("_ whitespace after single underscore_", TestName = "WhiteSpaceAfterSingleUnderscore")]
        [TestCase("__ whitespace after double underscore_", TestName = "WhiteSpaceAfterDoubleUnderscore")]
        public void GetNextSpanElement_NoSpanElementDecetected(string markdown)
        {
            parser = new MarkdownParser(markdown, spanElements);
            parser.DetermineSpanElement().Should().BeNull();            
        }

        [Test]
        public void GetTagClosingPosition_GetPlainText_RetunIndexBeforeClosingIndicator()
        {
            var markdown = "plain text before underscores_italic_";
            ISpanElement currentSpanElement = null;
            var closingSpanElement = StringIndexator.GetClosingIndex(markdown, 0, currentSpanElement, spanElements);
            closingSpanElement.Should().Be(28);
        }

        [Test]
        public void ParseNextTag_GetPlainText_ReturnWithoutFormatting()
        {
            var markdown = "_italic_hello world_italic_";
            parser = new MarkdownParser(markdown, spanElements);
            parser.ParseNextTag().Content.Should().Be("italic");
            parser.ParseNextTag().HasHtmlWrap().Should().BeFalse();
            parser.ParseNextTag().Content.Should().Be("italic");
        }

        [TestCase("some_italic_words", ExpectedResult = "italic")]
        [TestCase("some__bold__words", ExpectedResult = "bold")]
        public String ParseNextTag_GetMarkdownText_ReturnParsedTag(string markdown)
        {
            parser = new MarkdownParser(markdown, spanElements);
            parser.ParseNextTag();
            return parser.ParseNextTag().Content;
        }

        [Test]
        public void GetClosingIndex_WhiteSpaceBeforeClosingTag_SkipClosingTag()
        {
            var markdown = "_italic _text_";
            var closingSpanElement = StringIndexator.GetClosingIndex(markdown, 1, new SingleUnderscore(), spanElements);
            closingSpanElement.Should().Be(12);
        }

        [Test]
        public void ParseNextTag_EscapedUnderscore_DoNotParseTag()
        {
            var markdown = "some\\__italic_";
            parser = new MarkdownParser(markdown, spanElements);
            var tag = parser.DetermineSpanElement();
            tag.Should().BeNull();
        }


    }

}
