using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownToHtmlParserTests
    {
        [Test]
        [TestCaseSource(typeof(MarkdownToHtmlParserSource), "TestCases")]
        public void Parser_Should_ParseTokensToTokensCorrectly(MarkdownToHtmlParserSource.MarkdownToHtmlParserData data)
        {
            var expectedResult = data.Result;
            var dict = new Dictionary<Token, Token>();
            var htmltokens = MarkdownToHtmlParser.Parse(data.MdTokens, dict);
            htmltokens.Select(t => t.Line)
                .ToList()
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        public static class MarkdownToHtmlParserSource
        {
            public class MarkdownToHtmlParserData
            {
                public List<Token> MdTokens { get; }
                public List<string> Result { get; }
                public MarkdownToHtmlParserData(List<Token> mdTokens,params string[] tokenLines)
                {

                    MdTokens = mdTokens;
                    Result = tokenLines.ToList();
                }
            }

            private static readonly TestCaseData[] TestCases =
            {
                    new TestCaseData(new MarkdownToHtmlParserData(new List<Token>() {"_aaa_".GetToken(0,4,"_")}, "<em>aaa</em>")),
                    new TestCaseData(new MarkdownToHtmlParserData( new List<Token>()
                    {
                        "ab_aaa_bb__b a__ acc".GetToken(2,6,"_"),
                        "ab_aaa_bb__b a__ acc".GetToken(9,15,"__")
                    }, "<em>aaa</em>", "<strong>b a</strong>")),
                    new TestCaseData(new MarkdownToHtmlParserData(new List<Token>()
                    {
                        "ab__aaa__bb_b_".GetToken(2,8,"__"),
                        "ab__aaa__bb_b_".GetToken(11,13,"_"),

                    }, "<strong>aaa</strong>", "<em>b</em>")),
                    new TestCaseData(new MarkdownToHtmlParserData(new List<Token>(){"[link](somelink)".GetToken(0,15,"[](")}, @"<a href='somelink'>link</a>"))
            };
        }
    }
}
