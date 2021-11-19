using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class LexerTests
    {
        private Lexer sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Lexer();
        }

        [Test]
        public void Lex_ShouldThrowException_WhenTextIsNull()
        {
            string text = null;

            var tokens = sut.Lex(text);

            Assert.Throws<ArgumentNullException>(() => tokens.ToArray());
        }

        [TestCase("", TestName = "Empty string")]
        [TestCase("T", TestName = "Single character")]
        [TestCase("Text", TestName = "Single word")]
        [TestCase("Text with spaces", TestName = "Several words")]
        public void Lex_ShouldReturnTextToken_WhenTextWithoutFormatting(string text)
        {
            var tokens = sut.Lex(text);

            tokens.Should().Contain(new Token(TokenType.Text, text)).And.HaveCount(1);
        }

        [TestCaseSource(typeof(LexerTests), nameof(GetCursiveFormattingTestCases))]
        [TestCaseSource(typeof(LexerTests), nameof(GetBoldFormattingTestCases))]
        [TestCaseSource(typeof(LexerTests), nameof(GetEscapeFormattingTestCases))]
        [TestCaseSource(typeof(LexerTests), nameof(GetHeaderFormattingTestCases))]
        [TestCaseSource(typeof(LexerTests), nameof(GetNewLineFormattingTestCases))]
        public void Lex_ShouldReturnCorrectTokens_WhenTextWithSpecialCharacters(string text, Token[] expectedTokens)
        {
            var tokens = sut.Lex(text);

            tokens.Should().Equal(expectedTokens);
        }

        public static IEnumerable<TestCaseData> GetCursiveFormattingTestCases() =>
            GetFormattingTestCases("_", new Token(TokenType.Cursive, "_"));

        public static IEnumerable<TestCaseData> GetBoldFormattingTestCases() =>
            GetFormattingTestCases("__", new Token(TokenType.Bold, "__"));

        public static IEnumerable<TestCaseData> GetEscapeFormattingTestCases() =>
            GetFormattingTestCases("\\", new Token(TokenType.Escape, "\\"));

        public static IEnumerable<TestCaseData> GetHeaderFormattingTestCases() =>
            GetFormattingTestCases("#", new Token(TokenType.Header1, "#"));

        public static IEnumerable<TestCaseData> GetNewLineFormattingTestCases() =>
            GetFormattingTestCases("\n", new Token(TokenType.NewLine, "\n"));

        [Test]
        public void Lex_ShouldReturnTokens_WhenOnlySpecialSymbols()
        {
            var expected = new[]
            {
                new Token(TokenType.Bold, "__"),
                new Token(TokenType.Cursive, "_"),
                new Token(TokenType.Escape, "\\"),
                new Token(TokenType.Header1, "#")
            };
            var tokens = sut.Lex(string.Join("", expected.Select(t => t.Value)));

            tokens.Should().Equal(expected);
        }

        [TestCase(10_000, 1_000)]
        [TestCase(100_000, 1_000)]
        [TestCase(1_000_000, 3_000)]
        [TestCase(10_000_000, 5_000)]
        public void Lex_ShouldBeFast(int characterCount, long maxMilliseconds)
        {
            var rnd = new Random();
            var characters = new[] { 'a', '\\', '\n', '#', '_' };
            var text = string.Join("", Enumerable.Range(0, characterCount)
                .Select(x => characters[rnd.Next(characters.Length)]));
            GC.Collect();

            var sw = Stopwatch.StartNew();
            foreach (var _ in sut.Lex(text))
            {
            }

            sw.Stop();

            sw.ElapsedMilliseconds.Should().BeLessOrEqualTo(maxMilliseconds);
        }

        private static IEnumerable<TestCaseData> GetFormattingTestCases(string character, Token token)
        {
            yield return new TestCaseData(character, new[]
            {
                token
            }).SetName($"{character} only");

            yield return new TestCaseData($"start{character}", new[]
            {
                new Token(TokenType.Text, "start"),
                token
            }).SetName($"{character} after text start");

            yield return new TestCaseData($"{character}end", new[]
            {
                token,
                new Token(TokenType.Text, "end")
            }).SetName($"{character} before text end");

            yield return new TestCaseData($"{character}text{character}", new[]
            {
                token,
                new Token(TokenType.Text, "text"),
                token
            }).SetName($"{character} surrounds all text");
        }
    }
}