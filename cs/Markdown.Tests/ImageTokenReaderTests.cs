using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ImageTokenReaderTests
    {
        private ImageTokenReader Reader { get; set; }

        [SetUp]
        public void SetUp()
        {
            Reader = new ImageTokenReader();
        }

        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("![]()", 0,
                    new Token(0, "![]()", 4, TokenType.Image)
                        { ChildTokens =
                        {
                            new Token(0, "", 0, TokenType.PlainText),
                            new Token(1, "", 1, TokenType.PlainText)
                        }}
                ).SetName("Empty image");

                yield return new TestCaseData("![alt text](url)", 0,
                    new Token(0, "![alt text](url)", 15, TokenType.Image)
                    { ChildTokens =
                    {
                        new Token(0, "alt text", 0, TokenType.PlainText),
                        new Token(1, "url", 1, TokenType.PlainText)
                    }}
                ).SetName("Image with attributes");
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