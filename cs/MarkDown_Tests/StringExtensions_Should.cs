using FluentAssertions;
using MarkDown;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class StringExtensions_Should
    {
        [TestCase("aaa", 0, -1, ExpectedResult = '\0', TestName = "when less then 0")]
        [TestCase("aaa", 0, 1, ExpectedResult = 'a', TestName = "when bounds are correct")]
        [TestCase("aaa", 2, 1, ExpectedResult = '\0', TestName = "when larger then string length")]
        public char SeekFromPosition_ShouldReturnCorrectValue(string text, int currentPosition, int delta) =>
            text.SeekFromPosition(currentPosition, delta);

        [Test]
        public void RemoveScreening_RemovesScreeningCharacters()
        {
            "aaa\\aa".RemoveScreening().Should().Be("aaaaa");
        }

        [TestCase("_AAA_", 0, "_", ExpectedResult = true, TestName = "when string at start position is opening tag")]
        [TestCase("_AAA_", 2, "_", ExpectedResult = false, TestName = "when string at start position is letter")]
        [TestCase("_AAA_", 4, "_", ExpectedResult = false, TestName = "when string at start position is closing tag")]
        [TestCase("__AAA__", 0, "__", ExpectedResult = true, TestName = "when string at start position is opening tag")]
        [TestCase("__AAA__", 2, "__", ExpectedResult = false, TestName = "when string at start position is letter")]
        [TestCase("__AAA__", 4, "__", ExpectedResult = false, TestName = "when string at start position is closing tag")]
        public bool IsOpeningTag_RecognizeTagCorrect(string text, int startPositionToCheck, string specialSymbol)=> 
            text.IsOpeningTag(startPositionToCheck, specialSymbol);


        [TestCase("_AAA_", 0, "_", ExpectedResult = false, TestName = "when string at start position is opening tag")]
        [TestCase("_AAA_", 2, "_", ExpectedResult = false, TestName = "when string at start position is letter")]
        [TestCase("_AAA_", 4, "_", ExpectedResult = true, TestName = "when string at start position is closing tag")]
        [TestCase("__AAA__", 0, "__", ExpectedResult = false, TestName = "when string at start position is opening tag")]
        [TestCase("__AAA__", 2, "__", ExpectedResult = false, TestName = "when string at start position is letter")]
        [TestCase("__AAA__", 5, "__", ExpectedResult = true, TestName = "when string at start position is closing tag")]
        public bool IsClosingTag_RecognizeTagCorrect(string text, int startPosition, string specialSymbol) =>
            text.IsClosingTag(startPosition, specialSymbol);
        
    }
}
