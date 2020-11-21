using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HeadingTokenReaderTests
    {
        private HeadingTokenReader Reader { get; set; }

        [SetUp]
        public void SetUp()
        {
            Reader = new HeadingTokenReader();
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("# one", 0,
                    new Token(0, " one", 4, TokenType.Heading)
                ).SetName("One heading");

                yield return new TestCaseData("# one\n# two\n# three", 6,
                    new Token(6, " two", 10, TokenType.Heading)
                ).SetName("Heading in text");
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