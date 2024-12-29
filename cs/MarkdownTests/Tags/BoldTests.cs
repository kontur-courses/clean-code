using Markdown.Tags;
using FluentAssertions;

namespace MarkdownTests.Tags
{
    public class BoldTests
    {
        [Test]
        public void BoldTag_ShouldCorrectCalculateTagEnd()
        {
            var openTag = new Bold("__Вы__a", 0);

            openTag.TryCloseTag(3, "__Вы__a", out int tagEnd);

            tagEnd.Should().Be(5);
        }


        [TestCase("__12__", false, TestName = "ReturnFalse_WhenInsideDigits")]
        [TestCase("__нач__але",  true, TestName = "ReturnTrue_WhenSelectPartOfWord")]
        [TestCase("в ра__зных сл__овах",  false, TestName = "ReturnFalse_WhenSelectPartsOfDifferentWords1")]
        [TestCase("в раз__ных словах__", false, TestName = "ReturnFalse_WhenSelectPartsOfDifferentWords3")]
        [TestCase("в __разных словах__", true, TestName = "ReturnTrue_WhenSelectWholeWords")]
        [TestCase("в __ разных словах__",  false, TestName = "ReturnFalse_WhenHasSpaceAfterOpenTag")]
        public void AcceptIfContextCorrect_Should(string mdText, bool expected)
        {
            var tagStart = mdText.IndexOf('_');
            var contextEnd = mdText.LastIndexOf('_') - 2;
            var contextStart = tagStart + 2;
            var openTag = new Bold(mdText, tagStart);
            var isContextCorrect = true;
            for (var i = contextStart; i <= contextEnd; i++)
            {
                isContextCorrect = isContextCorrect && openTag.AcceptIfContextCorrect(i);
            }
            
            isContextCorrect.Should().Be(expected);
        }
    }
}
