using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class TokenReaderTest
    {
        [Test]
        public void TryParseText_ShouldReturnFalse_WhenBeginEqualsEnd()
        {
            Token token;
            TokenReader.TryParseText("some text", 10, 10, out token).Should().BeFalse();
        }
        
        [Test]
        public void TryParseText_ShouldReturnFalse_WhenEndLessThenBegin()
        {
            Token token;
            TokenReader.TryParseText("some text", 10, 8, out token).Should().BeFalse();
        }
        
        [Test]
        public void TryParseText_ShouldReturnFalse_WhenBeginLessThenZero()
        {
            Token token;
            TokenReader.TryParseText("some text", -1, 10, out token).Should().BeFalse();
        }
        
        [Test]
        public void TryParseText_ShouldReturnFalse_WhenEndGreaterThenInputLength()
        {
            Token token;
            TokenReader.TryParseText("some text", 2, 100, out token).Should().BeFalse();
        }
        
        [Test]
        public void TryParseText_ShouldSetCorrectToken()
        {
            Token token;
            TokenReader.TryParseText("some text", 0, 9, out token).Should().BeTrue();
            token.Length.Should().Be(9);
            token.Position.Should().Be(0);
        }

        [TestCase("_24_", 0, 4, ExpectedResult = true)]
        [TestCase("24", 0, 2, ExpectedResult = true)]
        [TestCase("2__4_2_33_", 0, 10, ExpectedResult = true)]
        [TestCase("text", 0, 4, ExpectedResult = false)]
        [TestCase("2_ 2_33_", 0, 8, ExpectedResult = false)]
        public bool IsNumberWithUnderlines_ShouldReturnCorrectResult_When(string input, int begin, int end)
        {
            return TokenReader.IsNumberWithUnderlines(input, begin, end);
        }
        
        [TestCase("_text_", ExpectedResult = true)]
        [TestCase("_text with spaces_", ExpectedResult = true)]
        [TestCase("_a 1_", ExpectedResult = true)]
        [TestCase("_1 a_", ExpectedResult = true)]
        [TestCase("_1_2_3 4_", ExpectedResult = true)]
        [TestCase("_ text_", ExpectedResult = false)]
        [TestCase("_text _", ExpectedResult = false)]
        [TestCase("__text__", ExpectedResult = false)]
        public bool TryParseSingleUnderlineTag_ShouldReturnCorrectResultAndSetCorrectToken_When(string input)
        {
            var begin = 0;
            var end = input.Length;
            Token token;
            
            if (TokenReader.TryParseSingleUnderlineTag(input, begin, end, out token))
            {
                token.Position.Should().Be(begin);
                token.Length.Should().Be(end - begin);
                token.ContentLeftOffset.Should().Be(1);
                token.ContentLength.Should().Be(token.Length - 2);
                return true;
            }

            return false;
        }
        
        [TestCase("__text__", ExpectedResult = true)]
        [TestCase("__text with spaces__", ExpectedResult = true)]
        [TestCase("__a 1__", ExpectedResult = true)]
        [TestCase("__1 a__", ExpectedResult = true)]
        [TestCase("__1_2_3 4__", ExpectedResult = true)]
        [TestCase("__ text__", ExpectedResult = false)]
        [TestCase("__text __", ExpectedResult = false)]
        [TestCase("_text_", ExpectedResult = false)]
        public bool TryParseDoubleUnderlineTag_ShouldReturnCorrectResultAndSetCorrectToken_When(string input)
        {
            var begin = 0;
            var end = input.Length;
            Token token;
            
            if (TokenReader.TryParseDoubleUnderlineTag(input, begin, end, out token))
            {
                token.Position.Should().Be(begin);
                token.Length.Should().Be(end - begin);
                token.ContentLeftOffset.Should().Be(2);
                token.ContentLength.Should().Be(token.Length - 4);
                return true;
            }

            return false;
        }
    }
}