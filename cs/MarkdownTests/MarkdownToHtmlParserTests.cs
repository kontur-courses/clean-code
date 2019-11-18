using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MarkdownTests
{
    public class MarkdownToHtmlParserTests
    {
        [Test]
        [TestCaseSource(typeof(MarkdownToHtmlParserSource), "TestCases")]
        public void Parser_Should_ParseTokensToTokensCorrectly(MarkdownToHtmlParserSource.MarkdownToHtmlParserData data)
        {
            var expectedResult = data.Result;
            var textTokens = data.TextTokens;
            var dict = new Dictionary<Token, Token>();
            var htmltokens = MarkdownToHtmlParser.Parse(textTokens, dict);
            htmltokens.Select(t => t.Line)
                .ToList()
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        public static class MarkdownToHtmlParserSource
        {
            public class MarkdownToHtmlParserData
            {
                public string Text { get; }
                public HashSet<Token> TextTokens { get; }
                public List<string> Result { get; }
                public MarkdownToHtmlParserData(string text, params string[] tokenLines)
                {
                    Text = text;
                    TextTokens = TextToTokensParser.Parse(text);
                    Result = tokenLines.ToList();
                }
            }

            private static readonly TestCaseData[] TestCases =
            {
                    new TestCaseData(new MarkdownToHtmlParserData("ab_aaa_bbb", "<em>aaa</em>")).SetName("ab_aaa_bbb"),
                    new TestCaseData(new MarkdownToHtmlParserData("ab_aaa_bb__b a__ acc", "<em>aaa</em>", "<strong>b a</strong>")).SetName(
                        "ab_aaa_bb__b a__ acc"),
                    new TestCaseData(new MarkdownToHtmlParserData("ab__aaa__bb_b_", "<strong>aaa</strong>", "<em>b</em>")).SetName("ab__aaa__bb_b_"),
                    new TestCaseData(new MarkdownToHtmlParserData("ab_aaa_bb_b_", "<em>aaa</em>", "<em>b</em>")).SetName("ab_aaa_bb_b_"),
                    new TestCaseData(new MarkdownToHtmlParserData("aaa__a_b_b__[link](somelink)", "<strong>a<em>b</em>b</strong>",
                            "<em>b</em>",@"<a href='somelink'>link</a>")).SetName("aaa__a_b_b__[link](somelink)")
                };
        }
    }
}
