using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
            public HashSet<string> Result { get; }
            public TextParserData(string text, params string[] tokenLines)
            {
                Text = text;
                Result = tokenLines.ToHashSet();
            }
        }

        private static readonly TestCaseData[] TestCases =
        {           new TestCaseData(new TextParserData("ab1_1aaa_bbb")).SetName("ab1_1aaa_bbb"),
                    new TestCaseData(new TextParserData("ab_aaa1_1bbb")).SetName("ab_aaa1_1bbb"),
                    new TestCaseData(new TextParserData("ab_ aaa_bbb")).SetName("ab_ aaa_bbb"),
                    new TestCaseData(new TextParserData("ab_aaa_bbb", "_aaa_")).SetName("ab_aaa_bbb"),
                    new TestCaseData(new TextParserData("ab_aaa_bb__b a__ acc", "_aaa_", "__b a__")).SetName(
                        "ab_aaa_bb__b a__ acc"),
                    new TestCaseData(new TextParserData("ab__aaa__bb_b_", "__aaa__", "_b_")).SetName("ab__aaa__bb_b_"),
                    new TestCaseData(new TextParserData("ab_aaa_bb_b_", "_aaa_", "_b_")).SetName("ab_aaa_bb_b_"),
                    new TestCaseData(new TextParserData("ab_aa__b__a_", "_aa__b__a_")).SetName("ab_aa__b__a_"),
                    new TestCaseData(new TextParserData("ab__aa_b_a__", "__aa_b_a__","_b_")).SetName("ab__aa_b_a__"),
                    new TestCaseData(new TextParserData("aaa__a_b_b__[link](somelink)", "__a_b_b__","_b_","[link](somelink)")).SetName("aaa__a_b_b__[link](somelink)")
                };
    }
}