using System.Collections.Generic;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class PlainTextTokenReaderTests
    {
        private PlainTextTokenReader reader;

        [SetUp]
        public void SetUp()
        {
            reader = new PlainTextTokenReader();
        }

        [TestCaseSource(nameof(TestCases))]
        public void TryReadToken_ReturnExpectedResult_When(string text, int position, Token expectedToken)
        {
            reader.TryReadToken(text, text, position, out var token);

            token.Should().BeEquivalentTo(expectedToken);
        }

        private static IEnumerable<TestCaseData> TestCases()
        {
            yield return new TestCaseData("text", 0, new PlaintTextToken(0, "text", 3)).SetName("Only plain text");
            yield return new TestCaseData("text __s__", 0, new PlaintTextToken(0, "text ", 4)).SetName(
                "Plain text at the start");
            yield return new TestCaseData("_e_ text", 3, new PlaintTextToken(3, " text", 7)).SetName(
                "Plain text at the end");
            yield return new TestCaseData("_e", 0, new PlaintTextToken(0, "_e", 1)).SetName("No end tag");
            yield return new TestCaseData("e_", 0, new PlaintTextToken(0, "e_", 1)).SetName("No start tag");
            yield return new TestCaseData("_1_", 0, new PlaintTextToken(0, "_1_", 2)).SetName("Digits");
            yield return new TestCaseData("__", 0, new PlaintTextToken(0, "__", 1)).SetName("Empty tags");
            yield return new TestCaseData("o_ne tw_o", 1, new PlaintTextToken(1, "_ne tw", 6)).SetName(
                "Different words");
            yield return new TestCaseData("_e\\_", 0, new PlaintTextToken(0, "_e_", 3)).SetName("Backslash at the end");
            yield return new TestCaseData("_e\ne_", 0, new PlaintTextToken(0, "_e\ne_", 4)).SetName("New line");
            yield return new TestCaseData("\\_e_", 0, new PlaintTextToken(0, "_e_", 3)).SetName(
                "Backslash at the start");
        }
    }
}