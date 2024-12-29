using Markdown.Tags;
using FluentAssertions;

namespace MarkdownTests.Tags
{
    public class EscapeTests
    {
        [Test]
        public void EscapeTag_ContextShould_ContainsOneSymbol()
        {
            var openTag = new Escape("\\_", 0);

            openTag.TryCloseTag(1, "\\_", out int tagEnd);

            tagEnd.Should().Be(1);
        }

        [TestCase("\\_", 0, true, TestName = "EscapeAnotherTag")]
        [TestCase("\\\\", 0, true, TestName = "EscapeItSelf")]
        [TestCase("\\a", 0, false, TestName = "HaveNoSymbolsForEscape")]
        public void EscapeTag_HaveCorrectContext_WhenEscapeMarkupSymbol(string markdownText, int tagStart, bool expected)
        {
            var openTag = new Escape(markdownText, tagStart);

            var isContextCorrect = openTag.AcceptIfContextCorrect(1);

            isContextCorrect.Should().Be(expected);
        }
    }
}
