using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class HtmlConverter_Should
    {
        private static readonly HtmlConverter htmlConverter = new();
        
        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TextNoTags))]
        public void ReturnStringItselfNoTags(string paragraph)
        {
            htmlConverter.InsertTags([new ParsedText(paragraph, [])]).Should().Be(paragraph);
        }
        
        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TagsWithText))]
        public void CorrectlyHandlesTagsWithText(string paragraph, List<ITag> tags, string expected)
        {
            htmlConverter.InsertTags([new ParsedText(paragraph, tags)]).Should().Be(expected);
        }

        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TagsWithTextDifferentParagraphs))]
        public void CorrectlyHandlesTagsWithTextFewParagraphs(ParsedText[] parsedTexts, string expected)
        {
            htmlConverter.InsertTags(parsedTexts).Should().Be(expected);
        }
    }
}
