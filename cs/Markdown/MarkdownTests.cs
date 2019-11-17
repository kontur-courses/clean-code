using  NUnit.Framework;
using  FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;

namespace Markdown
{
    public class MarkdownTests
    {
        [TestFixture]
        public class TextToTokensParserTests
        {
            [Test]
            [TestCaseSource(typeof(TextParserSource), "TestCases")]
            public void Parser_Should_ParseTokensCorrectly(TextParserSource.TextParserData data)
            {
                var text = data.Text;
                var expectedResult = data.Result;
                var result = TextToTokensParser.Parse(text);
                result.Select(t => t.Line)
                      .ToList()
                      .Should()
                      .BeEquivalentTo(expectedResult);
            }

            public static class TextParserSource
            {
                public  class TextParserData
                {
                    public string Text { get; }
                    public  HashSet<string> Result { get; }
                    public TextParserData(string text, params string[] tokenLines)
                    {
                        Text = text;
                        Result = tokenLines.ToHashSet();
                    }
                }

                private static readonly TestCaseData[] TestCases =
                {
                    new TestCaseData(new TextParserData("ab_aaa_bbb", "_aaa_")).SetName("ab_aaa_bbb"),
                    new TestCaseData(new TextParserData("ab_aaa_bb__b a__ acc", "_aaa_", "__b a__")).SetName(
                        "ab_aaa_bb__b a__ acc"),
                    new TestCaseData(new TextParserData("ab__aaa__bb_b_", "__aaa__", "_b_")).SetName("ab__aaa__bb_b_"),
                    new TestCaseData(new TextParserData("ab_aaa_bb_b_", "_aaa_", "_b_")).SetName("ab_aaa_bb_b_"),
                    new TestCaseData(new TextParserData("ab_aa__b__a_", "_aa__b__a_")).SetName("ab_aa__b__a_"),
                    new TestCaseData(new TextParserData("ab__aa_b_a__", "__aa_b_a__","_b_")).SetName("ab__aa_b_a__")
                };
            }
        }

        [TestFixture]
        public class MarkdownToHtmlParserTests
        {
            [Test]
            [TestCaseSource(typeof(MarkdownToHtmlParserSource),"TestCases")]
            public void Parser_Should_ParseTokensToTokensCorrectly(MarkdownToHtmlParserSource.MarkdownToHtmlParserData data)
            {
                var expectedResult = data.Result;
                var textTokens = data.TextTokens;
               var dict=new Dictionary<Token,Token>();
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
                    new TestCaseData(new MarkdownToHtmlParserData("ab_aaa_bb_b_", "<em>aaa</em>", "<em>b</em>")).SetName("ab_aaa_bb_b_")
                };
            }
        }

        public class RenderTests
        {
            [TestCase("ab__aa_b_a__", ExpectedResult = "ab<strong>aa<em>b</em>a</strong>")]
            [TestCase("ab__aaa__bb_b_",ExpectedResult = "ab<strong>aaa</strong>bb<em>b</em>")]
            [TestCase("ab_a aa_bb__b__", ExpectedResult = "ab<em>a aa</em>bb<strong>b</strong>")]
            [TestCase("ab_aa__b__a_",ExpectedResult = "ab<em>aa__b__a</em>")]
            public string Render_ShouldRenderCorrectly1(string text)
            {
                return new Md().Render(text);
            }
        }
    }
}
