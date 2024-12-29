using FluentAssertions;
using Markdown.Tags;

namespace MarkdownTests.Tags
{
    public class ItalicTests
    {
        [Test]
        public void ItalicTag_ShouldCorrectCalculateTagEnd()
        {
            var openTag = new Italic("_Вы_", 0);

            openTag.TryCloseTag(2, "_Вы_", out int tagEnd, []);

            tagEnd.Should().Be(3);
        }

        [TestCase("_12_", false, TestName = "ReturnFalse_WhenInsideDigits")]
        [TestCase("_нач_але", true, TestName = "ReturnTrue_WhenSelectPartOfWord")]
        [TestCase(" _нач_але", true, TestName = "ReturnTrue_WhenSelectPartOfWordAfterSpace")]
        [TestCase("в ра_зных сл_овах", false, TestName = "ReturnFalse_WhenSelectPartsOfDifferentWords1")]
        [TestCase("в раз_ных словах_", false, TestName = "ReturnFalse_WhenSelectPartsOfDifferentWords3")]
        [TestCase("в _разных словах_", true, TestName = "ReturnTrue_WhenSelectWholeWords")]
        [TestCase("в _ разных словах_", false, TestName = "ReturnFalse_WhenHasSpaceAfterOpenTag")]
        public void AcceptIfContextCorrect_Should(string mdText, bool expected)
        {
            var tagStart = mdText.IndexOf('_');
            var contextEnd = mdText.LastIndexOf('_') - 1;
            var contextStart = tagStart + 1;
            var openTag = new Italic(mdText, tagStart);
            var isContextCorrect = true;
            for (var i = contextStart; i <= contextEnd; i++)
            {
                isContextCorrect = isContextCorrect && openTag.AcceptIfContextCorrect(i);
            }

            isContextCorrect.Should().Be(expected);
        }
    }
}
