using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TextWorkerTests
    {
        private TextWorker textWorker;

        [SetUp]
        public void SetUp()
        {
            textWorker = new TextWorker(new[] {'_', '#'});
        }

        [Test]
        public void IsEscapedChar_WhenOnlyOneEscapeChar_ReturnTrue()
        {
            textWorker.IsEscapedChar(@"\a", 1).Should().BeTrue();
        }

        [Test]
        public void IsEscapedChar_WhenNoEscapeCharacter_ReturnFalse()
        {
            textWorker.IsEscapedChar("a", 0).Should().BeFalse();
        }

        [TestCase(@"\\\\\a", 5, true, TestName = "CharIsEscaped_ReturnTrue")]
        [TestCase(@"\\\\a", 4, false, TestName = "CharIsNotEscaped_ReturnFalse")]
        public void IsEscapedChar_WhenMoreThanOneEscapeChar_ReturnTrue(
            string line, int index, bool expectedResult)
        {
            textWorker.IsEscapedChar(line, index).Should().Be(expectedResult);
        }

        [Test]
        public void IsThereDigit_WhenDigitInWord_ReturnTrue()
        {
            textWorker.IsThereDigit("abc1", 0, 3).Should().BeTrue();
        }

        [Test]
        public void IsThereDigit_WhenDigitInAnotherWord_ReturnTrue()
        {
            textWorker.IsThereDigit("a2 bc 1d", 3, 7).Should().BeTrue();
        }

        [Test]
        public void IsThereDigit_WhenInWordNoDigit_ReturnFalse()
        {
            textWorker.IsThereDigit("aa", 0, 1).Should().BeFalse();
        }

        [Test]
        public void IsStartOfWord_IfIndexPointsOnWhiteSpace_ReturnFalse()
        {
            textWorker.IsStartOfWord("  abc", 1).Should().BeFalse();
        }

        [Test]
        public void IsStartOfWord_IndexPointsOnSymbolAndBeforeIndexNoWordCharacter_ReturnTure()
        {
            textWorker.IsStartOfWord("_abc", 1).Should().BeTrue();
        }

        [Test]
        public void IsStartOfWord_IndexPointsOnSymbolAndBeforeIndexStandWhiteSpace_ReturnTrue()
        {
            textWorker.IsStartOfWord(" _abc", 1).Should().BeTrue();
        }

        [Test]
        public void IsStartOfWord_IndexIsZeroAndPointsOnNotWhiteSpace_ReturnTrue()
        {
            textWorker.IsStartOfWord("abc", 0).Should().BeTrue();
        }

        [Test]
        public void IsEndOfWord_IfIndexPointsOnWhiteSpace_ReturnFalse()
        {
            textWorker.IsEndOfWord("ab ", 2).Should().BeFalse();
        }

        [Test]
        public void IsEndOfWord_IndexPointsOnSymbolAndAfterIndexNoWordCharacter_ReturnTrue()
        {
            textWorker.IsEndOfWord("ab_", 1).Should().BeTrue();
        }

        [Test]
        public void IsEndOfWord_IndexPointsOnSymbolAndAfterIndexStandWhiteSpace_ReturnTrue()
        {
            textWorker.IsEndOfWord("ab ", 1).Should().BeTrue();
        }

        [Test]
        public void IsEndOfWord_IndexIsPointsOnEndOfLineAndItIsNotWhiteSpace_ReturnTrue()
        {
            textWorker.IsEndOfWord("abc", 2).Should().BeTrue();
        }

        [Test]
        public void DeleteEscapeCharFromLine_DeleteOnlySomethingEscapingChars()
        {
            var line = @"Здесь сим\волы экранирования\ \должны остаться.\";

            textWorker.DeleteEscapeCharFromLine(line).Should().Be(line);
        }

        [Test]
        public void DeleteEscapeCharFromLine_EscapeCharCanBeEscaped()
        {
            var line = @"\\_вот это будет выделено тегом_";

            textWorker.DeleteEscapeCharFromLine(line).Should()
                .Be(@"\_вот это будет выделено тегом_");
        }

        [Test]
        public void DeleteEscapeCharFromLine_IfEscapeCharEscapingSomething_DeleteIt()
        {
            var line = @"\_Вот это\_";

            textWorker.DeleteEscapeCharFromLine(line).Should()
                .Be(@"_Вот это_");
        }

        [Test]
        public void GoThroughText_IfFirstIndexLessThanLastIndex_ReturnInRegularlyOrder()
        {
            var line = "abcd";

            textWorker.GoThroughText(line, 0, 2).Should()
                .BeEquivalentTo("abc", options => options.WithStrictOrdering());
        }

        [Test]
        public void GoThroughText_IfFirstIndexMoreThanLastIndex_ReturnInReverseOrder()
        {
            var line = "abcd";

            textWorker.GoThroughText(line, 2, 0).Should()
                .BeEquivalentTo("cba", options => options.WithStrictOrdering());
        }

        [TestCase("в _нач_але", 2, 6, TestName = "IndexesIncludeStartOfWord")]
        [TestCase("в сер_еди_не", 5, 9, TestName = "IndexesIncludeMiddleOfWord")]
        [TestCase("в кон_це._", 5, 9, TestName = "IndexesIncludeEndOfWord")]
        public void InTwoDifferentWords_IfIndexesInOneWord_ReturnFalse(
            string line, int firstIndex, int secondIndex)
        {
            textWorker.InTwoDifferentWords(line, firstIndex, secondIndex).Should().BeFalse();
        }

        [TestCase("_ab _c", 0, 4, TestName = "IndexesAtBorderOfWords")]
        [TestCase("ab_c d_b", 2, 6, TestName = "IndexesInsideDifferentWords")]
        public void InTwoDifferentWords_IndexesInDifferentWord_ReturnTrue(
            string line, int firstIndex, int secondIndex)
        {
            textWorker.InTwoDifferentWords(line, firstIndex, secondIndex).Should().BeTrue();
        }
    }
}