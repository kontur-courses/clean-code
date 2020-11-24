using System.Collections.Generic;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class StrongTokenReaderTests
    {
        private StrongTokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new StrongTokenReader();
        }

        [TestCaseSource(nameof(TestCases))]
        public void TryReadToken_ReturnExpectedResult_When(string text, int position, Token expectedToken)
        {
            reader.TryReadToken(text, text, position, out var token);

            token.Should().BeEquivalentTo(expectedToken);
        }

        [TestCaseSource(nameof(WrongTestCases))]
        public void TryReadToken_ShouldBeFalse_When(string text, int position)
        {
            reader.TryReadToken(text, text, position, out var token).Should().BeFalse();
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData("__word__", 0, new StrongToken(0, "word", 7)).SetName("One token");
            yield return new TestCaseData("__long text__", 0, new StrongToken(0, "long text", 12)).SetName(
                "More than one words in one tag");
            yield return new TestCaseData("__s _e_ s__", 0, new StrongToken(0, "s _e_ s", 10)).SetName(
                "Emphasized tag in strong");
            yield return new TestCaseData("__sta__rt", 0, new StrongToken(0, "sta", 6)).SetName("In word start");
            yield return new TestCaseData("mi__dd__le", 2, new StrongToken(2, "dd", 7)).SetName("In word middle");
            yield return new TestCaseData("en__d.__", 2, new StrongToken(2, "d.", 7)).SetName("In word end");
            yield return new TestCaseData(@"__e\\__", 0, new StrongToken(0, @"e\\", 6)).SetName(
                "Backslashes at the end");
            yield return new TestCaseData(@"__e\\\\\\__", 0, new StrongToken(0, @"e\\\\\\", 10)).SetName(
                "Many backslashes at the end");
            yield return new TestCaseData(@"__e __e__ e__", 0, new StrongToken(0, @"e __e__ e", 12)).SetName(
                "Strong tag in strong");
            yield return new TestCaseData(@"__e _e_ e__", 0, new StrongToken(0, @"e _e_ e", 10)).SetName(
                "Emphasized tag in strong");
            yield return new TestCaseData(@"__a __b __c __f__ c__ b__ a__", 0,
                new StrongToken(0, @"a __b __c __f__ c__ b__ a", 28)).SetName("Deep nesting");
        }

        private static IEnumerable<TestCaseData> WrongTestCases()
        {
            yield return new TestCaseData("__e __e__", 0).SetName("No strong end tag");
            yield return new TestCaseData("e__ e__", 1).SetName("No strong start tag");
            yield return new TestCaseData("__12__3", 0).SetName("Digits in tag");
            yield return new TestCaseData("____", 0).SetName("Empty tag");
            yield return new TestCaseData("o__ne tw__o", 1).SetName("Different words");
            yield return new TestCaseData("__s\\__", 0).SetName("Backslash at the end");
            yield return new TestCaseData("__s\ns__", 0).SetName("New line");
            yield return new TestCaseData("__s\r\ns__", 0).SetName("Non-unix new line");
        }
    }
}