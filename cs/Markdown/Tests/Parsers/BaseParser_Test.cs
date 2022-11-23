using FluentAssertions;
using Markdown.Parsers;
using NUnit.Framework;

namespace Markdown.Tests.Parsers
{
    [TestFixture]
    public class BaseParser_Test : BaseParser
    {
        private BaseParser_Test sut;

        [OneTimeSetUp]
        public void StartTests()
        {
            sut = this;
        }
        
        [Test]
        public void GetState_ShouldIncreaseState_WhenFindNextTagChar()
        {
            var state = 0;
            state = sut.GetState(tag.Close[0], state);
            state.Should().Be(1);
            state = sut.GetState(tag.Close[1], state);
            state.Should().Be(2);
        }

        [Test]
        public void GetState_ShouldReturnZero_WhenTestingCharIsIncorrect()
        {
            sut.GetState('f', 2).Should().Be(0);
        }

        [Test]
        public void IsOpenInWord_ShouldReturnTrue_WhenTagOpenInWord()
        {
            sut.IsOpenInWord(2, "wo"+tag.Open+"rd").Should().BeTrue();
        }
        
        [Test]
        public void IsOpenInWord_ShouldReturnFalse_WhenTagOpenBeforeWord()
        {
            sut.IsOpenInWord(0, tag.Open+"word").Should().BeFalse();
        }
        
        [Test]
        public void IsOpenInWord_ShouldReturnFalse_WhenTagOpenAfterWordAndSpace()
        {
            sut.IsOpenInWord(5, "word "+tag.Open).Should().BeFalse();
        }

        [Test]
        public void IsEmptySelection_ShouldReturnTrue_WhenSubstringBetweenOpenAndCloseTagsIsEmpty()
        {
            // example "_*_*"
            sut.IsEmptySelection(0, 3).Should().BeTrue();
        }
        
        [Test]
        public void IsEmptySelection_ShouldReturnFalse_WhenSubstringBetweenOpenAndCloseTagsIsNotEmpty()
        {
            // example "_*text_*"
            sut.IsEmptySelection(0, 7).Should().BeFalse();
        }

        [TestCase("_* ", 0, ExpectedResult = true, TestName = "Space_AfterTag")]
        [TestCase("_*", 0, ExpectedResult = true, TestName = "TextEnd_AfterTag")]
        [TestCase("_*\n", 0, ExpectedResult = true, TestName = "NewLine_AfterTag")]
        [TestCase("_*text", 0, ExpectedResult = false, TestName = "Letter_AfterTag")]
        public bool NextCharAfterTag_IsSpaceOrEndLine_Test(string text, int position)
        {
            return sut.NextCharAfterTag_IsSpaceOrEndLine(position, text, "_*");
        }

        [TestCase("not_*number_*", 3, 12, ExpectedResult = false)]
        [TestCase("_*123_*", 0, 6, ExpectedResult = false)]
        [TestCase("_*text_*1", 0, 7, ExpectedResult = false)]
        [TestCase("1_*text_*", 1, 8, ExpectedResult = false)]
        [TestCase("1_*23_*", 1, 6, ExpectedResult = true)]
        [TestCase("_*12_*3", 0, 5, ExpectedResult = true)]
        public bool IsIntoNumber_Test(string text, int tokenStart, int tokenEnd)
        {
            return sut.IsIntoNumber(tokenStart, tokenEnd, text);
        }
        
        #region BaseParser extention
        // Необходимо для тестирования protected методов
        public BaseParser_Test() : base(new Tag("_*", "_*"))
        {
        }

        public override Token TryParseTag(int position, string text)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}