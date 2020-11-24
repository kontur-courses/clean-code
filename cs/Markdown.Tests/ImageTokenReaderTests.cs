using System.Collections.Generic;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ImageTokenReaderTests
    {
        private ImageTokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new ImageTokenReader();
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
            yield return new TestCaseData(
                "![]()",
                0,
                new ImageToken(0, "![]()", 4)
                {
                    ChildTokens =
                    {
                        new PlaintTextToken(1, "", 2),
                        new PlaintTextToken(3, "", 4)
                    }
                }).SetName("Empty image");
            yield return new TestCaseData(
                "![alt text](url)",
                0,
                new ImageToken(0, "![alt text](url)", 15)
                {
                    ChildTokens =
                    {
                        new PlaintTextToken(1, "alt text", 10),
                        new PlaintTextToken(11, "url", 15)
                    }
                }).SetName("Image with attributes");
            yield return new TestCaseData(
                @"![c\\at](https://i.ibb.co/fxwGJZB/image.png)",
                0,
                new ImageToken(0, @"![c\at](https://i.ibb.co/fxwGJZB/image.png)", 42)
                {
                    ChildTokens =
                    {
                        new PlaintTextToken(1, @"c\\at", 7),
                        new PlaintTextToken(8, "https://i.ibb.co/fxwGJZB/image.png", 43)
                    }
                }).SetName("Alt text with backslashes");
        }

        private static IEnumerable<TestCaseData> WrongTestCases()
        {
            yield return new TestCaseData("![\\]()", 0).SetName("Backslash before alt text end tag");
            yield return new TestCaseData("!\\[]()", 0).SetName("Backslash before alt text start tag");
            yield return new TestCaseData("![](\\)", 0).SetName("Backslash before closing tag");
            yield return new TestCaseData("![](\n)", 0).SetName("New line in tag");
            yield return new TestCaseData("![]t()", 0).SetName("Text between tags");
            yield return new TestCaseData("![] ()", 0).SetName("White space between tags");
            yield return new TestCaseData("![\r\n]()", 0).SetName("Non-unix new line in tag");
        }
    }
}