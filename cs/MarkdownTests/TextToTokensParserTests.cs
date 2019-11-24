using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;


namespace MarkdownTests
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
            public class TextParserData
            {
                public string Text { get; }
                public List<string> Result { get; }

                public TextParserData(string text, params string[] tokenLines)
                {
                    Text = text;
                    Result = tokenLines.ToList();
                }
            }

            private static readonly TestCaseData[] TestCases =
            {
                new TestCaseData(new TextParserData("__[l](ll)__","__[l](ll)__","[l](ll)")).SetName("__[l](ll)__"), 
                new TestCaseData(new TextParserData(@"\__a\__","_a\\__")).SetName(@"\__a\__"), 
                new TestCaseData(new TextParserData("ab1_1aaa_bbb")).SetName("ab1_1aaa_bbb"),
                new TestCaseData(new TextParserData("[link] (some)")).SetName("[link] (some)"),
                new TestCaseData(new TextParserData("a _b_ a", "_b_")).SetName("a _b_ a"),
                new TestCaseData(new TextParserData("a_b_a", "_b_")).SetName("a_b_a"),
                new TestCaseData(new TextParserData(@"\\\\_a_", "_a_")).SetName(@"\\\\_a_"),
                new TestCaseData(new TextParserData(@"\\\\\_a_")).SetName(@"\\\\\_a_"),
                new TestCaseData(new TextParserData("__a_")).SetName("__a_"),
                new TestCaseData(new TextParserData("_a__", "_a_")).SetName("_a__"),
                new TestCaseData(new TextParserData("1__2_")).SetName("1__2_"),
                new TestCaseData(new TextParserData("1_2__")).SetName("1_2__"),
                new TestCaseData(new TextParserData("____", "____")).SetName("____"),
                new TestCaseData(new TextParserData(@"_\_a\__", @"_\_a\__")).SetName(@"_\_a\__"),
                new TestCaseData(new TextParserData("__")).SetName("__"),
                new TestCaseData(new TextParserData("_a__a_b_", "_a_", "_a_")).SetName("_a__a_b_"),
                new TestCaseData(new TextParserData(@"\_a_")).SetName(@"\_a_"),
                new TestCaseData(new TextParserData("_a_a__b_", "_a_")).SetName("_a_a__b_"),
                new TestCaseData(new TextParserData(@"_a\_")).SetName(@"_a\_"),
                new TestCaseData(new TextParserData("ab_aaa1_1bbb")).SetName("ab_aaa1_1bbb"),
                new TestCaseData(new TextParserData("ab_ aaa_bbb")).SetName("ab_ aaa_bbb"),
                new TestCaseData(new TextParserData("ab_aaa_bbb", "_aaa_")).SetName("ab_aaa_bbb"),
                new TestCaseData(new TextParserData("ab_aaa_bb__b a__ acc", "_aaa_", "__b a__")).SetName(
                    "ab_aaa_bb__b a__ acc"),
                new TestCaseData(new TextParserData("ab__aaa__bb_b_", "__aaa__", "_b_")).SetName("ab__aaa__bb_b_"),
                new TestCaseData(new TextParserData("ab_aaa_bb_b_", "_aaa_", "_b_")).SetName("ab_aaa_bb_b_"),
                new TestCaseData(new TextParserData("ab_aa__b__a_", "_aa_", "_b_", "_a_")).SetName("ab_aa__b__a_"),
                new TestCaseData(new TextParserData("ab__aa_b_a__", "__aa_b_a__", "_b_")).SetName("ab__aa_b_a__"),
                new TestCaseData(new TextParserData("aaa__a_b_b__[link](somelink)", "__a_b_b__", "_b_",
                    "[link](somelink)")).SetName("aaa__a_b_b__[link](somelink)")
            };
        }
    }
}