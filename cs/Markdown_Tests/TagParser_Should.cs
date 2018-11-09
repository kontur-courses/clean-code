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
            parser = new MarkdownParser();
        }

        [Test]
        public void SplitMarkdownOnTags_GetPlainText_ReturnWithoutChanges()
        {
            var markdown = "plain text";
            var tags = parser.ParseMarkdownOnHtmlTags(markdown, spanElements);
            tags.First().HasHtmlWrap().Should().BeFalse();
        }

        [Test]
        public void GetNextSpanElement_GetPlainText_NoSpanElementDetected()
        {
            var markdown = "plain text";
            for (int i = 0; i < markdown.Length; i++)
                parser.DetermineSpanElement(markdown, i, spanElements).Should().BeNull();
        }

        [TestCase("_single underscore_", typeof(SingleUnderscore), TestName = "DetectSingleUnderscoreSpanElement")]
        [TestCase("__double underscore__", typeof(DoubleUnderscore), TestName = "DetectDoubleUnderscoreSpanElement")]
        public void GetNextSpanElement_GetFormattedText(string markdown, Type type)
        {
            var span = parser.DetermineSpanElement(markdown, 0, spanElements);
            span.Should().BeOfType(type);
        }

        [TestCase("_ whitespace after single underscore_", TestName = "WhiteSpaceAfterSingleUnderscore")]
        [TestCase("__ whitespace after double underscore_", TestName = "WhiteSpaceAfterDoubleUnderscore")]
        public void GetNextSpanElement_NoSpanElementDecetected(string markdown)
        {
            parser.DetermineSpanElement(markdown, 0, spanElements).Should().BeNull();            
        }

        [Test]
        public void GetTagClosingPosition_GetPlainText_RetunIndexBeforeClosingIndicator()
        {
            var text = "plain text before underscores_italic_";
            ISpanElement currentSpanElement = null;
            var closingSpanElement = parser.GetSpanElementClosingIndex(text, 0, currentSpanElement, spanElements);
            closingSpanElement.Should().Be(28);
        }

        [Test]
        public void ParseNextTag_GetPlainText_ReturnWithoutFormatting()
        {
            var text = "_italic_hello world_italic_";
            var tag = parser.ParseNextTag(text, 8, spanElements);
            tag.Content.Should().Be("hello world");
        }

        [TestCase("some_italic_words", ExpectedResult = "italic")]
        [TestCase("some__bold__words", ExpectedResult = "bold")]
        public String ParseNextTag_GetItalicText_ReturnItalicTag(string markdown)
        {
            var tag = parser.ParseNextTag(markdown, 4, spanElements);
            return tag.Content;
        }

        [Test]
        public void GetSpanElementClosingIndex_WhiteSpaceBeforeClosingTag_SkipClosingTag()
        {
            var text = "_italic _text_";
            var closingSpanElement = parser.GetSpanElementClosingIndex(text, 1, new SingleUnderscore(), spanElements);
            closingSpanElement.Should().Be(12);
        }

        [Test]
        public void ParseNextTag_EscapedUnderscore_DoNotParseTag()
        {
            var text = "some\\__italic_";
            var tag = parser.DetermineSpanElement(text, 5, spanElements);
            tag.Should().BeNull();
            parser.Position.Should().Be(5);
        }

    }

}
