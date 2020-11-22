using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlCreatorTests
    {
        [TestCase("_abc_", "<em>", 0, "_", "<em>abc_", TestName = "AddCorrectOpeningItalicTag")]
        [TestCase("_abc_", "</em>", 4, "_", "_abc</em>", TestName = "AddCorrectClosingItalicTag")]
        [TestCase("__abc__", "<strong>", 0, "__", "<strong>abc__", TestName = "AddCorrectOpeningBoldTag")]
        [TestCase("__abc__", "</strong>", 5, "__", "__abc</strong>", TestName = "AddCorrectClosingBoldTag")]
        [TestCase("#abc", "<h1>", 0, "#", "<h1>abc", TestName = "AddCorrectOpeningTitleTag")]
        [TestCase("#abc", "</h1>", 4, null, "#abc</h1>", TestName = "AddCorrectClosingTitleTag")]
        public void AddHtmlTagToTextTest(string text, string tagValue, int position, string markdown, string expected)
        {
            var actual =
                HtmlCreator.AddHtmlTagToText(text, new Tag(tagValue, text, position, markdown));
            actual.Should().Be(expected);
        }
    }
}