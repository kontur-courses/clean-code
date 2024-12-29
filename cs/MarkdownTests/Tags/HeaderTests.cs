using FluentAssertions;
using Markdown.Tags;

namespace MarkdownTests.Tags
{
    public class HeaderTests
    {
        [TestCase("#Вы fhjg\n", 7, 8)]
        [TestCase("#Вы fhjg", 7, 7, TestName = "AndHasNotSymbolForParagraphEnd")]
        public void HeaderTag_ShouldClose_WhenParagraphEnd(string mdText, int contextEnd, int expected)
        {
            var openTag = new Header(mdText, 0);

            openTag.TryCloseTag(contextEnd, mdText, out var tagEnd);

            tagEnd.Should().Be(expected);
        }
    }
}
