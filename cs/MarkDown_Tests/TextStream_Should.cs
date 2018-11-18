using System.Linq;
using FluentAssertions;
using MarkDown;
using NUnit.Framework;
// ReSharper disable StringLiteralTypo

namespace MarkDown_Tests
{
    public class TextStream_Should
    {
        private TextStream textStream;
        [SetUp]
        public void SetUp()
        {
            textStream = new TextStream("just");   
        }

        [Test]
        public void HaveCorrectCurrentPosition_AfterCreation()
        {
            textStream.CurrentPosition.Should().Be(0);
        }        
        
        [Test]
        public void HaveCorrectCurrentLength_AfterCreation()
        {
            textStream.Length.Should().Be(4);
        }

        [Test]
        public void TryMoveNext_And_ReturnTrue_WhenInBounds()
        {
            textStream.TryMoveNext().Should().BeTrue();
        }        
        
        [Test]
        public void TryMoveNext_And_MoveCurrentPosition_WhenInBounds()
        {
            textStream.TryMoveNext(2);
            textStream.CurrentPosition.Should().Be(2);
        }
        
        [Test]
        public void TryMoveNext_And_ReturnFalse_WhenOutOfBounds()
        {
            textStream.TryMoveNext(5).Should().BeFalse();
        }  
        
        [Test]
        public void TryMoveNext_And_NotMoveCurrentPosition_WhenOutOfBounds()
        {
            textStream.TryMoveNext(5);
            textStream.CurrentPosition.Should().Be(0);
        }

        [Test]
        public void TryGetSubstring_And_ReturnTrue_WhenInBounds()
        {
            textStream.TryGetSubstring(0, 2, out var res).Should().BeTrue();
        }
        
        [Test]
        public void TryGetSubstring_And_WriteResultToOut_WhenInBounds()
        {
            textStream.TryGetSubstring(0, 2, out var res);
            res.Should().Be("ju");
        }        
        
        [Test]
        public void TryGetSubstring_And_ReturnFalse_WhenOutOfBounds()
        {
            textStream.TryGetSubstring(0, 5, out var res).Should().BeFalse();
            textStream.TryGetSubstring(-1, 3, out var res1).Should().BeFalse();
            textStream.TryGetSubstring(-1, 5, out var res2).Should().BeFalse();
        }
        
        [Test]
        public void TryGetSubstring_And_WriteEmptyStringInOut_WhenOutOfBounds()
        {
            textStream.TryGetSubstring(0, 5, out var res);
            textStream.TryGetSubstring(-1, 3, out var res1);
            textStream.TryGetSubstring(-1, 5, out var res2);
            res.Should().BeEmpty();
            res1.Should().BeEmpty();
            res2.Should().BeEmpty();
        }

        [TestCase("f1x _a_ 1g", ExpectedResult = true, TestName = "when text with numbers is outside")]
        [TestCase("fddx_a_ffg", ExpectedResult = true, TestName = "when inside text without numbers")]
        [TestCase("fddx_1_ffg", ExpectedResult = true, TestName = "when text with numbers only inside tag")]
        [TestCase("f1xx_a_1gg", ExpectedResult = false, TestName = "when inside text with numbers")]
        public bool Check_IsTokenAtCurrentNumberLess_And_ReturnExpected(string text)
        {
            textStream = new TextStream(text);
            textStream.TryMoveNext(4);
            return textStream.IsTokenAtCurrentNumberLess(6);
        }

        [TestCase("_aaa_", 0, "_", ExpectedResult = true, TestName = "when symbol at position is single undersocre opening")]
        [TestCase("_aaa_", 1, "_", ExpectedResult = false, TestName = "when symbol at position is not special symbol")]
        [TestCase("_aaa_", 4, "_", ExpectedResult = false, TestName = "when symbol at position is single underscore closing")]
        [TestCase("__aaa__", 1, "_", ExpectedResult = false, TestName = "when symbol at position is part of another special symbol")]
        [TestCase("__aaa__", 0, "__", ExpectedResult = true, TestName = "when symbol at position is double underscore opening")]
        [TestCase("__aaa__", 5, "__", ExpectedResult = false, TestName = "when symbol at position is double underscore closing")]
        public bool Check_IsCurrentOpening_And_ReturnExpected(string text, int toMove, string specialSymbol)
        {
            var symbols = new []{"_", "__"}.Where(s => s != specialSymbol);
            textStream = new TextStream(text);
            textStream.TryMoveNext(toMove);
            return textStream.IsCurrentOpening(specialSymbol, symbols);
        }

        [TestCase("_aaa_", 0, "_", ExpectedResult = false, TestName = "when symbol at position is single undersocre opening")]
        [TestCase("_aaa_", 1, "_", ExpectedResult = false, TestName = "when symbol at position is not special symbol")]
        [TestCase("_aaa_", 4, "_", ExpectedResult = true, TestName = "when symbol at position is single underscore closing")]
        [TestCase("__aaa__", 5, "_", ExpectedResult = false, TestName = "when symbol at position is part of another special symbol")]
        [TestCase("__aaa__", 0, "__", ExpectedResult = false, TestName = "when symbol at position is double underscore opening")]
        [TestCase("__aaa__", 5, "__", ExpectedResult = true, TestName = "when symbol at position is double underscore closing")]
        public bool Check_IsSymbolAtPositionClosing(string text, int position, string specialSymbol)
        {
            var symbols = new []{"_", "__"}.Where(s => s != specialSymbol);
            textStream = new TextStream(text);
            return textStream.IsSymbolAtPositionClosing(position, specialSymbol, symbols);
        }
    }
}