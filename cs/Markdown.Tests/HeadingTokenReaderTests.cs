using System.Collections.Generic;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HeadingTokenReaderTests
    {
        private HeadingTokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new HeadingTokenReader();
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
            yield return new TestCaseData("# one", 0, new HeadingToken(0, "one", 4)).SetName("One heading");
            yield return new TestCaseData("# one\n# two\n# three", 6, new HeadingToken(6, "two", 10)).SetName(
                "Heading in text");
            yield return new TestCaseData("# unix\r\n", 0, new HeadingToken(0, "unix", 5)).SetName(
                "Non-unix platform end");
            yield return new TestCaseData("# h __s _e_ s__\n", 0, new HeadingToken(0, "h __s _e_ s__", 14)).SetName(
                "Deep nesting");
            yield return new TestCaseData("# ", 0, new HeadingToken(0, "", 1)).SetName("Empty heading");
        }

        private static IEnumerable<TestCaseData> WrongTestCases()
        {
            yield return new TestCaseData("text # h", 5).SetName("Not at the start");
            yield return new TestCaseData("dot. \ne# t", 7).SetName("Not at the start of new line");
            yield return new TestCaseData("_e_# t", 3).SetName("After tag");
            yield return new TestCaseData("\\# t", 1).SetName("After backslash");
            yield return new TestCaseData("#t", 0).SetName("No white space after heading");
            yield return new TestCaseData("#", 0).SetName("Only heading");
        }
    }
}