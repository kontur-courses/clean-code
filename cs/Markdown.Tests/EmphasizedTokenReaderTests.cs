using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class EmphasizedTokenReaderTests
    {
        private EmphasizedTokenReader Reader { get; set; }

        [SetUp]
        public void SetUp()
        {
            Reader = new EmphasizedTokenReader();
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("_word_", 0,
                    new Token(0, "word", 5, TokenType.Emphasized)
                ).SetName("One token");

                yield return new TestCaseData("_long text_", 0,
                    new Token(0, "long text", 10, TokenType.Emphasized)
                ).SetName("Two words in one tag");

                yield return new TestCaseData("_sta_rt", 0,
                    new Token(0, "sta", 4, TokenType.Emphasized)
                ).SetName("In word start");

                yield return new TestCaseData("mi_dd_le", 2,
                    new Token(2, "dd", 5, TokenType.Emphasized)
                ).SetName("In word middle");

                yield return new TestCaseData("en_d._", 2,
                    new Token(2, "d.", 5, TokenType.Emphasized)
                ).SetName("In word end");

                yield return new TestCaseData(@"_e\\_", 0,
                    new Token(0, @"e\\", 4, TokenType.Emphasized)
                ).SetName("Backslashes at the end");

                yield return new TestCaseData(@"_e\\\\\\_", 0,
                    new Token(0, @"e\\\\\\", 8, TokenType.Emphasized)
                ).SetName("Many backslashes at the end");

                yield return new TestCaseData(@"_e _e_ e_", 0,
                    new Token(0, @"e _e_ e", 8, TokenType.Emphasized)
                ).SetName("Emphasized tag in emphasized");

                yield return new TestCaseData(@"_e __e__ e_", 0,
                    new Token(0, @"e __e__ e", 10, TokenType.Emphasized)
                ).SetName("Strong tag in emphasized");

                yield return new TestCaseData(@"_a _b _c _f_ c_ b_ a_", 0,
                    new Token(0, @"a _b _c _f_ c_ b_ a", 20, TokenType.Emphasized)
                ).SetName("Deep nesting");
            }
        }

        private static IEnumerable<TestCaseData> WrongTestCases
        {
            get
            {
                yield return new TestCaseData("_e _e_", 0).SetName("No emphasized end tag");
                yield return new TestCaseData("e_ e_", 1).SetName("No emphasized start tag");
                yield return new TestCaseData("_12_3", 0).SetName("Digits in tag");
                yield return new TestCaseData("__", 0).SetName("Empty tag");
                yield return new TestCaseData("o_ne tw_o", 1).SetName("Different words");
                yield return new TestCaseData(@"_e\_", 1).SetName("Backslash at the end");
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