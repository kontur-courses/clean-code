using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;


namespace Markdown.Tests
{
    [TestFixture]
    class MdTokenParser_Should
    {
        [Test]
        [TestCaseSource(nameof(GetTestCaseData))]
        public void TokenizeCorrectly(string text, List<Token> expectedResult)
        {
            var tokenizer = new MdTokenParser();
            tokenizer.Tokenize(text).Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<TestCaseData> GetTestCaseData()
        {
            yield return new TestCaseData(@"abc", new List<Token>
            {
                new Token(TokenType.Text, @"abc")
            }).SetName("text only");

            yield return new TestCaseData(@"_abc", new List<Token>
            {
                new Token(TokenType.Italic),
                new Token(TokenType.Text, @"abc")
            }).SetName("text and italic symbol");

            yield return new TestCaseData(@"__abc", new List<Token>
            {
                new Token(TokenType.Bold),
                new Token(TokenType.Text, @"abc")
            }).SetName("text and bold symbol");

            yield return new TestCaseData(@"\_abc", new List<Token>
            {
                new Token(TokenType.Text, @"_abc", 5)
            }).SetName("escaped italic symbol");

            yield return new TestCaseData(@"\_\_abc", new List<Token>
            {
                new Token(TokenType.Text, @"__abc", 7)
            }).SetName("escaped bold symbols");

            yield return new TestCaseData(@"\_\_abc", new List<Token>
            {
                new Token(TokenType.Text, @"__abc", 7)
            }).SetName("escaped bold symbols");

            yield return new TestCaseData(@"\_\_abc", new List<Token>
            {
                new Token(TokenType.Text, @"__abc", 7)
            }).SetName("escaped bold symbols");

            yield return new TestCaseData(@"#abc", new List<Token>
            {
                new Token(TokenType.H1),
                new Token(TokenType.Text, @"abc")
            }).SetName("h1");

            yield return new TestCaseData(@"##abc", new List<Token>
            {
                new Token(TokenType.H2),
                new Token(TokenType.Text, @"abc")
            }).SetName("h2");

            yield return new TestCaseData(@"###abc", new List<Token>
            {
                new Token(TokenType.H3),
                new Token(TokenType.Text, @"abc")
            }).SetName("h3");

            yield return new TestCaseData(@"####abc", new List<Token>
            {
                new Token(TokenType.H4),
                new Token(TokenType.Text, @"abc")
            }).SetName("h4");

            yield return new TestCaseData(@"#####abc", new List<Token>
            {
                new Token(TokenType.H5),
                new Token(TokenType.Text, @"abc")
            }).SetName("h5");

            yield return new TestCaseData(@"######abc", new List<Token>
            {
                new Token(TokenType.H6),
                new Token(TokenType.Text, @"abc")
            }).SetName("h6");

            yield return new TestCaseData(@"##__abc_def_ghi__", new List<Token>
            {
                new Token(TokenType.H2),
                new Token(TokenType.Bold),
                new Token(TokenType.Text, @"abc"),
                new Token(TokenType.Italic),
                new Token(TokenType.Text, @"def"),
                new Token(TokenType.Italic),
                new Token(TokenType.Text, @"ghi"),
                new Token(TokenType.Bold),
            }).SetName("multiple different tags");
        }
    }
}