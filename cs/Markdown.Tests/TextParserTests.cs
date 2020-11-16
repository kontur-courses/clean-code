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
            new TestCaseData("_word_", new List<Token>()
            {
                new Token(0, "_word_", TokenType.Emphasized)
            }).SetName("One token"),

            new TestCaseData("_long text_", new List<Token>()
            {
                new Token(0, "_long text_", TokenType.Emphasized),
            }).SetName("Two words in one tag"),

            new TestCaseData("_sta_rt mi_dd_le en_d._", new List<Token>()
            {
                new Token(0, "_sta_", TokenType.Emphasized),
                new Token(5, "rt mi", TokenType.PlainText),
                new Token(10, "_dd_", TokenType.Emphasized),
                new Token(14, "le en", TokenType.PlainText),
                new Token(19, "_d._", TokenType.Emphasized),
            }).SetName("Token in words"),

            new TestCaseData("_e __s__ e_", new List<Token>()
            {
                new Token(0, "_e __s__ e_", TokenType.Emphasized),
            }).SetName("Strong tag in emphasized"),

            new TestCaseData("_in tag_ text", new List<Token>()
            {
                new Token(0, "_in tag_", TokenType.Emphasized),
                new Token(8, " text", TokenType.PlainText),
            }).SetName("Plain text at the end"),
        };

        [TestCaseSource("EmphasizedTestCases")]
        public void GetTokens_ReturnEmphasizedTokens_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

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

            new TestCaseData("__long text with spaces__", new List<Token>()
            {
                new Token(0, "__long text with spaces__", TokenType.Strong),
            }).SetName("More than two words in one tag"),

            new TestCaseData("__s s _e_ s__", new List<Token>()
            {
                new Token(0, "__s s _e_ s__", TokenType.Strong),
            }).SetName("Emphasized tag in strong"),

            new TestCaseData("text __in tag__", new List<Token>()
            {
                new Token(0, "text ", TokenType.PlainText),
                new Token(5, "__in tag__", TokenType.Strong),
            }).SetName("Plain text at start"),
        };

        [TestCaseSource("StrongTestCases")]
        public void GetTokens_ReturnStrongTokens_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] HeadingTestCases =
        {
            new TestCaseData("# one", new List<Token>()
            {
                new Token(0, "# one", TokenType.Heading)
            }).SetName("One heading"),

            new TestCaseData("# one\n# two\n# three", new List<Token>()
            {
                new Token(0, "# one", TokenType.Heading),
                new Token(5, "\n", TokenType.PlainText),
                new Token(6, "# two", TokenType.Heading),
                new Token(11, "\n", TokenType.PlainText),
                new Token(12, "# three", TokenType.Heading)
            }).SetName("More than one heading"),
        };

        [TestCaseSource("HeadingTestCases")]
        public void GetTokens_ReturnHeadingTokens_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] PlainTextTestCases =
        {
            new TestCaseData("", new List<Token>() {}).SetName("Empty string"),

            new TestCaseData("_ empty_", new List<Token>()
            {
                new Token(0, "_ empty_", TokenType.PlainText),
            }).SetName("White space after opening tag"),

            new TestCaseData("__empty __", new List<Token>()
            {
                new Token(0, "__empty __", TokenType.PlainText),
            }).SetName("White space before closing tag"),

            new TestCaseData("__12__3", new List<Token>()
            {
                new Token(0, "__12__3", TokenType.PlainText),
            }).SetName("Digits in tags"),

            new TestCaseData("__No_ paired", new List<Token>()
            {
                new Token(0, "__No_ paired", TokenType.PlainText),
            }).SetName("Not paired"),

            new TestCaseData("____", new List<Token>()
            {
                new Token(0, "____", TokenType.PlainText),
            }).SetName("Empty strong tag"),

            new TestCaseData("_E __s e_ S__", new List<Token>()
            {
                new Token(0, "_E __s e_ S__", TokenType.PlainText),
            }).SetName("Emphasized tag intersect strong tag"),

            new TestCaseData("__S _e s__ E_", new List<Token>()
            {
                new Token(0, "__S _e s__ E_", TokenType.PlainText),
            }).SetName("Strong tag intersect emphasized tag"),
        };

        [TestCaseSource("PlainTextTestCases")]
        public void GetTokens_ReturnPlainTextTokens_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] BackslashTestCases =
        {
            new TestCaseData(@"\_", new List<Token>()
            {
                new Token(0, @"\_", TokenType.PlainText),
            }).SetName("Tag after backslash"),

            new TestCaseData(@"_e\\_", new List<Token>()
            {
                new Token(0, @"_e\\_", TokenType.Emphasized),
            }).SetName("Escaped backslash before ending tag"),

            new TestCaseData(@"_e\_", new List<Token>()
            {
                new Token(0, @"_e\_", TokenType.PlainText),
            }).SetName("Backslash before ending tag"),
        };

        [TestCaseSource("BackslashTestCases")]
        public void GetTokens_ReturnExpectedResult_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        private static object[] ImageTestCases =
        {
            new TestCaseData(@"![]()", new List<Token>()
            {
                new Token(0, @"![]()", TokenType.Image),
            }).SetName("Only empty image tag"),
        };

        [TestCaseSource("ImageTestCases")]
        public void GetTokens_ReturnImageTag_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }
    }
}