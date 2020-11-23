using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class HtmlCreatorTests
    {
        [TestCase("_abc_", "<em>", 0, "_", ExpectedResult = "<em>abc_", TestName = "AddOpeningItalicTag")]
        [TestCase("_abc_", "</em>", 4, "_", ExpectedResult = "_abc</em>", TestName = "AddClosingItalicTag")]
        [TestCase("__abc__", "<strong>", 0, "__", ExpectedResult = "<strong>abc__", TestName = "AddOpeningBoldTag")]
        [TestCase("__abc__", "</strong>", 5, "__", ExpectedResult = "__abc</strong>", TestName = "AddClosingBoldTag")]
        [TestCase("#abc", "<h1>", 0, "#", ExpectedResult = "<h1>abc", TestName = "AddOpeningTitleTag")]
        [TestCase("#abc", "</h1>", 4, null, ExpectedResult = "#abc</h1>", TestName = "AddClosingTitleTag")]
        public string AddHtmlTagToTextTest(string text, string tagValue, int position, string markdown)
        {
            return
                HtmlCreator.AddHtmlTagToText(text, new Tag(tagValue, position, markdown));
        }
    }
}