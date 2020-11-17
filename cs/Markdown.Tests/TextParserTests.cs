using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TextParserTests
    {
        private TextParser Parser { get; set; }

        [SetUp]
        public void SetUp()
        {
            Parser = new TextParser();
        }

        private static object[] EmphasizedTestCases =
        {
            new TestCaseData("_e __s__ e_", new List<Token>()
            {
                new Token(0, "_e __s__ e_", TokenType.Emphasized),
                new Token(3, "__s__", TokenType.Strong),
            }).SetName("Strong tag in emphasized"),

            new TestCaseData("_in tag_ text", new List<Token>()
            {
                new Token(0, "_in tag_", TokenType.Emphasized),
                new Token(8, " text", TokenType.PlainText),
            }).SetName("Plain text at the end"),
        };

        [TestCaseSource(nameof(EmphasizedTestCases))]
        public void GetTokens_ReturnEmphasizedTokens_When(string text, List<Token> expectedTokens)
        {
            var result = Parser.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] StrongTestCases =
        {
            new TestCaseData("__let__ __me__ __in__", new List<Token>()
            {
                new Token(0, "__let__", TokenType.Strong),
                new Token(7, " ", TokenType.PlainText),
                new Token(8, "__me__", TokenType.Strong),
                new Token(14, " ", TokenType.PlainText),
                new Token(15, "__in__", TokenType.Strong),
            }).SetName("More than one token"),

            new TestCaseData("__s s _e_ s__", new List<Token>()
            {
                new Token(0, "__s s _e_ s__", TokenType.Strong),
                new Token(6, "_e_", TokenType.Emphasized),
            }).SetName("Emphasized tag in strong"),

            new TestCaseData("text __in tag__", new List<Token>()
            {
                new Token(0, "text ", TokenType.PlainText),
                new Token(5, "__in tag__", TokenType.Strong),
            }).SetName("Plain text at start"),
        };

        [TestCaseSource(nameof(StrongTestCases))]
        public void GetTokens_ReturnStrongTokens_When(string text, List<Token> expectedTokens)
        {
            var result = Parser.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] PlainTextTestCases =
        {
            new TestCaseData("_E __s e_ S__", new List<Token>()
            {
                new Token(0, "_E __s e_ S__", TokenType.PlainText),
            }).SetName("Emphasized tag intersect strong tag"),

            new TestCaseData("__S _e s__ E_", new List<Token>()
            {
                new Token(0, "__S _e s__ E_", TokenType.PlainText),
            }).SetName("Strong tag intersect emphasized tag"),
        };

        [TestCaseSource(nameof(PlainTextTestCases))]
        public void GetTokens_ReturnPlainTextTokens_When(string text, List<Token> expectedTokens)
        {
            var result = Parser.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] BackslashTestCases =
        {
            new TestCaseData(@"\_", new List<Token>()
            {
                new Token(1, @"_", TokenType.PlainText),
            }).SetName("Tag after backslash"),

            new TestCaseData(@"_e\\_", new List<Token>()
            {
                new Token(0, @"_e\_", TokenType.Emphasized),
            }).SetName("Escaped backslash before ending tag"),

            new TestCaseData(@"te\xt", new List<Token>()
            {
                new Token(0, @"te\xt", TokenType.PlainText),
            }).SetName("Not escaped backslash"),
        };

        [TestCaseSource(nameof(BackslashTestCases))]
        public void GetTokens_ReturnExpectedResult_When(string text, List<Token> expectedTokens)
        {
            var result = Parser.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }
    }
}