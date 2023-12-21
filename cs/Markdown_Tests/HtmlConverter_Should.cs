using FluentAssertions;
using Markdown;
using Markdown.Converters;
using Markdown.Tags;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class HtmlConverter_Should
    {
        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TextNoTags))]
        public void ReturnStringItselfNoTags(string paragraph)
        {
            HtmlConverter.InsertTags(new ParsedText(paragraph, [])).Should().Be(paragraph);
        }
        
        [TestCaseSource(typeof(TestHtmlConverterData), nameof(TestHtmlConverterData.TagsWithText))]
        public void CorrectlyHandlesTagsWithText(string paragraph, List<ITag> tags, string expected)
        {
            HtmlConverter.InsertTags(new ParsedText(paragraph, tags)).Should().Be(expected);
        }
    }
}
