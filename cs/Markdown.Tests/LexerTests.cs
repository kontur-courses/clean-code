using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class LexerTests
    {
        private Lexer.Lexer sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Lexer.Lexer();
        }

        [Test]
        public void Lex_ShouldThrowException_WhenTextIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Lex(null));
        }

        [TestCase("", TestName = "Empty string")]
        [TestCase("T", TestName = "Single character")]
        [TestCase("Text", TestName = "Single word")]
        [TestCase("Text with spaces", TestName = "Several words")]
        public void Lex_ShouldReturnTextToken_WhenTextWithoutFormatting(string text)
        {
            var tokens = sut.Lex(text).ToArray();

            tokens.Should().Contain(Token.Text(text)).And.HaveCount(1);
        }

        [TestCaseSource(nameof(GetCursiveFormattingTestCases))]
        [TestCaseSource(nameof(GetBoldFormattingTestCases))]
        [TestCaseSource(nameof(GetEscapeFormattingTestCases))]
        [TestCaseSource(nameof(GetHeaderFormattingTestCases))]
        [TestCaseSource(nameof(GetNewLineFormattingTestCases))]
        public void Lex_ShouldReturnCorrectTokens_WhenTextWithSpecialCharacters(string text, Token[] expectedTokens)
        {
            var tokens = sut.Lex(text);

            tokens.Should().Equal(expectedTokens);
        }

        public static IEnumerable<TestCaseData> GetCursiveFormattingTestCases() =>
            GetFormattingTestCases(Token.Cursive);

        public static IEnumerable<TestCaseData> GetBoldFormattingTestCases() =>
            GetFormattingTestCases(Token.Bold);

        public static IEnumerable<TestCaseData> GetEscapeFormattingTestCases() =>
            GetFormattingTestCases(Token.Escape);

        public static IEnumerable<TestCaseData> GetHeaderFormattingTestCases()
        {
            yield return new TestCaseData("# Text", new[]
            {
                Token.Header1,
                Token.Text("Text")
            }).SetName("# before text");

            yield return new TestCaseData("#Text", new[]
            {
                Token.Text("#Text")
            }).SetName("# not followed by whitespace");

            yield return new TestCaseData("Text#", new[]
            {
                Token.Text("Text#")
            }).SetName("# after text");

            yield return new TestCaseData("Text\n# Text", new[]
            {
                Token.Text("Text"),
                Token.NewLine,
                Token.Header1,
                Token.Text("Text")
            }).SetName("# on second line before text");

            var specialTokens = new[] { Token.Bold, Token.Cursive, Token.Escape };
            foreach (var token in specialTokens)
                yield return new TestCaseData($"{token.Value}# Text", new[]
                {
                    token,
                    Token.Text("# Text")
                }).SetName($"# on second line before text and after {token.Value}");

            yield return new TestCaseData("   # Text", new[]
            {
                Token.Text("   "),
                Token.Header1,
                Token.Text("Text")
            }).SetName("# after whitespaces before text");
        }

        public static IEnumerable<TestCaseData> GetNewLineFormattingTestCases() =>
            GetFormattingTestCases(Token.NewLine);

        [Test]
        public void Lex_ShouldReturnTokens_WhenOnlySpecialSymbols()
        {
            var expected = new[]
            {
                Token.Header1,
                Token.Bold,
                Token.Cursive,
                Token.Escape,
                Token.OpenCircleBracket,
                Token.CloseCircleBracket,
                Token.OpenSquareBracket,
                Token.CloseSquareBracket
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
            var characters = new[]
            {
                'a', Characters.Escape, Characters.Underscore, Characters.NewLine, Characters.Sharp,
                Characters.WhiteSpace
            };
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

        [Test]
        public void Lex_ShouldNotClearPreviousTokens_WhenCalledTwice()
        {
            var first = sut.Lex("A");
            var second = sut.Lex("B");

            first.Should().Contain(Token.Text("A"));
            second.Should().Contain(Token.Text("B"));
        }

        [Test]
        public void Lex_ShouldNotClearPreviousTokens_WhenCalledTwice_AndAlternately()
        {
            using var first = sut.Lex("A_").GetEnumerator();
            using var second = sut.Lex("B_").GetEnumerator();

            first.MoveNext();
            first.Current.Should().Be(Token.Text("A"));
            second.MoveNext();
            second.Current.Should().Be(Token.Text("B"));
            first.MoveNext();
            first.Current.Should().Be(Token.Cursive);
            second.MoveNext();
            second.Current.Should().Be(Token.Cursive);
        }

        private static IEnumerable<TestCaseData> GetFormattingTestCases(Token token)
        {
            var character = token.Value;
            yield return new TestCaseData(character, new[]
            {
                token
            }).SetName($"{character} only");

            yield return new TestCaseData($"start{character}", new[]
            {
                Token.Text("start"),
                token
            }).SetName($"{character} after text start");

            yield return new TestCaseData($"{character}end", new[]
            {
                token,
                Token.Text("end")
            }).SetName($"{character} before text end");

            yield return new TestCaseData($"{character}text{character}", new[]
            {
                token,
                Token.Text("text"),
                token
            }).SetName($"{character} surrounds all text");
        }
    }
}