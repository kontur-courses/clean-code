using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class StrongTokenReaderTests
    {
        private StrongTokenReader Reader { get; set; }

        [SetUp]
        public void SetUp()
        {
            Reader = new StrongTokenReader();
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("__word__", 0,
                    new Token(0, "word", 7, TokenType.Strong)
                ).SetName("One token");

                yield return new TestCaseData("__long text__", 0,
                    new Token(0, "long text", 12, TokenType.Strong)
                ).SetName("More than one words in one tag");

                yield return new TestCaseData("__s _e_ s__", 0,
                    new Token(0, "s _e_ s", 10, TokenType.Strong)
                ).SetName("Emphasized tag in strong");

                yield return new TestCaseData("__sta__rt", 0,
                    new Token(0, "sta", 6, TokenType.Strong)
                ).SetName("In word start");

                yield return new TestCaseData("mi__dd__le", 2,
                    new Token(2, "dd", 7, TokenType.Strong)
                ).SetName("In word middle");

                yield return new TestCaseData("en__d.__", 2,
                    new Token(2, "d.", 7, TokenType.Strong)
                ).SetName("In word end");

                yield return new TestCaseData(@"__e\\__", 0,
                    new Token(0, @"e\\", 6, TokenType.Strong)
                ).SetName("Backslashes at the end");

                yield return new TestCaseData(@"__e\\\\\\__", 0,
                    new Token(0, @"e\\\\\\", 10, TokenType.Strong)
                ).SetName("Many backslashes at the end");

                yield return new TestCaseData(@"__e __e__ e__", 0,
                    new Token(0, @"e __e__ e", 12, TokenType.Strong)
                ).SetName("Strong tag in strong");

                yield return new TestCaseData(@"__e _e_ e__", 0,
                    new Token(0, @"e _e_ e", 10, TokenType.Strong)
                ).SetName("Emphasized tag in strong");

                yield return new TestCaseData(@"__a __b __c __f__ c__ b__ a__", 0,
                    new Token(0, @"a __b __c __f__ c__ b__ a", 28, TokenType.Strong)
                ).SetName("Deep nesting");
            }
        }

        private static IEnumerable<TestCaseData> WrongTestCases
        {
            get
            {
                yield return new TestCaseData("__e __e__", 0).SetName("No strong end tag");
                yield return new TestCaseData("e__ e__", 1).SetName("No strong start tag");
                yield return new TestCaseData("__12__3", 0).SetName("Digits in tag");
                yield return new TestCaseData("____", 0).SetName("Empty tag");
                yield return new TestCaseData("o__ne tw__o", 1).SetName("Different words");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, Token expectedToken)
        {
            Reader.TryReadToken(text, text, position, out var token);

            token.Should().BeEquivalentTo(expectedToken);
        }

        [TestCaseSource(nameof(WrongTestCases))]
        public void TryReadToken_ShouldBeFalse_When(string text, int position)
        {
            Reader.TryReadToken(text, text, position, out var token).Should().BeFalse();
        }
    }
}