using System.Diagnostics;
using System.Text;
using FluentAssertions;
using MarkDown;
using MarkDown.TagParsers;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    internal class LineConverter_Tests
    {
        [SetUp]
        public void SetUp()
        {
            _parserGetter = new ParserGetter();
            _lineConverter = new LineConverter(_parserGetter);
        }

        private static LineConverter _lineConverter;
        private static ParserGetter _parserGetter;

        [TestCase("", TestName = "EmptyString")]
        [TestCase("hello", TestName = "CommonString")]
        [TestCase("_hello", TestName = "EmOpened")]
        [TestCase("__hello", TestName = "StrongOpened")]
        [TestCase("___hello", TestName = "EmStrongOpened")]
        [TestCase("hello_", TestName = "EmClosed")]
        [TestCase("hello__", TestName = "StrongClosed")]
        [TestCase("_hello _", TestName = "EmNotClosed")]
        [TestCase("__hello __", TestName = "StrongNotClosed")]
        [TestCase("_12_3", TestName = "ClosingEMInDigits")]
        [TestCase("_12_3", TestName = "ClosingEMInText")]
        [TestCase("_____", TestName = "LotsOfTagsWithout")]
        [TestCase("_hello__", TestName = "StrongOpenedEmClosed")]
        [TestCase("__hello_", TestName = "EmOpenStrongClosed")]
        public void LineParser_ReturnsSameText_On(string text)
        {
            _lineConverter.GetParsedLineFrom(text).Should().Be(text);
        }

        [TestCase("__a _a_ a__", "<strong>a <em>a</em> a</strong>", TestName =
            "2EmInStrong")]
        [TestCase("__again_again__ again_", "<strong>again_again</strong> again_", TestName =
            "1StrongInEm")]
        [TestCase("_hello __a__ a_", "<em>hello __a__ a</em>", TestName =
            "2StrongIn2Em")]
        [TestCase("__hello _a__ a_", "__hello <em>a__ a</em>", TestName =
            "2StrongIn2Em")]
        public void LineParser_CorrectString_(string text, string expected)
        {
            _lineConverter.GetParsedLineFrom(text).Should().Be(expected);
        }

        [TestCase(@"\_hello_", "_hello_", TestName = "")]
        [TestCase(@"_hello\_", "_hello_", TestName = "BackSlashBeforeEm")]
        [TestCase(@"__hello\__", "__hello__", TestName = "BackSlashBeforeStrong")]
        [TestCase(@"\\\\", @"\\", TestName = "EvenBackslashes")]
        [TestCase(@"\\\", @"\\", TestName = "OddBackslashes")]
        public void Translate_ReturnsCorrectString_OnTextWithBackslashes(string text, string expected)
        {
            _lineConverter.GetParsedLineFrom(text).Should().Be(expected);
        }

        private long GetWorkingTime(int countOfChainParts)
        {
            var text = BuildLongString(countOfChainParts);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _lineConverter.GetParsedLineFrom(text);
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }

        private static string BuildLongString(int countOfChainParts)
        {
            const string chainPart = "_a_ __b__ \\\\\\ _c__ d_ e__";
            var builder = new StringBuilder();
            for (var i = 0; i < countOfChainParts; i++)
                builder.Append(chainPart);
            return builder.ToString();
        }

        [Test]
        public void Translate_AlgorithmicComplexityIsCloseToLinear()
        {
            GetWorkingTime(400);
            var firstResult = GetWorkingTime(400);
            GetWorkingTime(800);
            var secondResult = GetWorkingTime(800);
            GetWorkingTime(200);
            var thirdResult = GetWorkingTime(200);
            GetWorkingTime(400);
            var forthResult = GetWorkingTime(400);
            (forthResult / thirdResult / (secondResult / firstResult)).Should().BeLessThan(2);
        }
    }
}