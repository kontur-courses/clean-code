using System.Collections.Generic;
using System.Linq;
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

        static object[] TestCases =
        {
            new TestCaseData("", new List<Token>() {}).SetName("Empty string"),

            new TestCaseData("# one", new List<Token>()
            {
                new Token(0, "# one", TokenType.Heading)
            }).SetName("One heading"),

            new TestCaseData("_word_", new List<Token>()
            {
                new Token(0, "_word_", TokenType.Emphasized)
            }).SetName("One token"),

            new TestCaseData("__let__ _me_ __in__", new List<Token>()
            {
                new Token(0, "__let__", TokenType.Strong),
                new Token(7, " ", TokenType.PlainText),
                new Token(8, "_me_", TokenType.Emphasized),
                new Token(12, " ", TokenType.PlainText),
                new Token(13, "__in__", TokenType.Strong),
            }).SetName("More than one token"),

            new TestCaseData("_long text_", new List<Token>()
            {
                new Token(0, "_long text_", TokenType.Emphasized),
            }).SetName("Two words in one tag"),

            new TestCaseData("_long text with spaces_", new List<Token>()
            {
                new Token(0, "_long text with spaces_", TokenType.Emphasized),
            }).SetName("More than two words in one tag"),

            new TestCaseData("_sta_rt mi__dd__le en_d._", new List<Token>()
            {
                new Token(0, "_sta_", TokenType.Emphasized),
                new Token(5, "rt mi", TokenType.PlainText),
                new Token(10, "__dd__", TokenType.Strong),
                new Token(16, "le en", TokenType.PlainText),
                new Token(21, "_d._", TokenType.Emphasized),
            }).SetName("Token in words"),

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

            new TestCaseData("text _in tag_", new List<Token>()
            {
                new Token(0, "text ", TokenType.PlainText),
                new Token(5, "_in tag_", TokenType.Emphasized),
            }).SetName("Plain text at start"),

            new TestCaseData("_in tag_ text", new List<Token>()
            {
                new Token(0, "_in tag_", TokenType.Emphasized),
                new Token(8, " text", TokenType.PlainText),
            }).SetName("Plain text at the end"),

            new TestCaseData("__No_ paired", new List<Token>()
            {
                new Token(0, "__No_ paired", TokenType.PlainText),
            }).SetName("Not paired"),

            new TestCaseData("____", new List<Token>()
            {
                new Token(0, "____", TokenType.PlainText),
            }).SetName("Empty strong tag"),

            new TestCaseData("__s s _e_ s__", new List<Token>()
            {
                new Token(0, "__s s _e_ s__", TokenType.Strong),
            }).SetName("Emphasized tag in strong"),

            new TestCaseData("_e __s__ e_", new List<Token>()
            {
                new Token(0, "_e __s__ e_", TokenType.Emphasized),
            }).SetName("Strong tag in emphasized"),

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

        [TestCaseSource("TestCases")]
        public void GetTokens_ReturnExpectedTokens_When(string text, List<Token> expectedTokens)
        {
            var result = new TextParser().GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }
    }
}