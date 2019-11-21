using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HTMLRenderer_Should
    {
        [Test]
        [TestCaseSource(nameof(GetTestCaseData))]
        public void RenderCorrectly(List<Token> tokens, string expectedResult)
        {
            var renderer = new HTMLRenderer();
            renderer.Render(tokens).Should().BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<TestCaseData> GetTestCaseData()
        {
            yield return new TestCaseData(new List<Token>
                {
                    new Token(TokenType.Text, @"abc")
                },
                "abc<br><br>").SetName("text only");
            yield return new TestCaseData(new List<Token>
                {
                    new PairToken(TokenType.Italic, "_", 1, true),
                    new PairToken(TokenType.Italic, "_", 1, false)
                },
                "<em></em><br><br>").SetName("italic tags");
            yield return new TestCaseData(new List<Token>
                {
                    new PairToken(TokenType.Bold, "__", 1, true),
                    new PairToken(TokenType.Bold, "__", 1, false)
                },
                "<strong></strong><br><br>").SetName("bold tags");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H1),
                },
                "<h1></h1>").SetName("h1 tag");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H2),
                },
                "<h2></h2>").SetName("h2 tag");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H3),
                },
                "<h3></h3>").SetName("h3 tag");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H4),
                },
                "<h4></h4>").SetName("h4 tag");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H5),
                },
                "<h5></h5>").SetName("h5 tag");
            yield return new TestCaseData(new List<Token>
                {
                    new HeaderToken(TokenType.H6),
                },
                "<h6></h6>").SetName("h6 tag");
        }
    }
}