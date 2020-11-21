using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class PlainTextTokenReaderTests
    {
        private PlainTextTokenReader Reader { get; set; }

        [SetUp]
        public void SetUp()
        {
            Reader = new PlainTextTokenReader();
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("text", 0,
                    new Token(0, "text", 3, TokenType.PlainText)
                    ).SetName("Only plain text");

                yield return new TestCaseData("text __s__", 0,
                    new Token(0, "text ", 4, TokenType.PlainText)
                ).SetName("Plain text at the start");

                yield return new TestCaseData("_e_ text", 3,
                    new Token(3, " text", 7, TokenType.PlainText)
                ).SetName("Plain text at the end");
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void TryReadToken_ReturnExpectedResult_When(
            string text, int position, Token expectedToken)
        {
            Reader.TryReadToken(text, text, position, out var token);

            token.Should().BeEquivalentTo(expectedToken);
        }
    }
}