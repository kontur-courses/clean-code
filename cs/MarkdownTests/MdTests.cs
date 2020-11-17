using System.Diagnostics;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Tests
    {
        private readonly Md md = new Md();

        [TestCase("_abc_", ExpectedResult = "<em>abc</em>", TestName = "When_Only_One_Tag")]
        public string Render_ConvertItalicTag(string input)
        {
            return md.Render(input);
        }

        [TestCase("# abc", ExpectedResult = "<h1>abc</h1>", TestName = "When_Only_One_Tag")]
        [TestCase("# abc\ndef", ExpectedResult = "<h1>abc</h1>\ndef", TestName = "When_With_Line_Break")]
        public string Render_ConvertHeaderTag(string input)
        {
            return md.Render(input);
        }

        [TestCase("[google](www.google.ru)",
            ExpectedResult = "<a href=\"www.google.ru\">google</a>",
            TestName = "When_Only_One_Tag")]
        public string Render_ConvertLinkTag(string input)
        {
            return md.Render(input);
        }

        [TestCase(@"\_abc_", ExpectedResult = "_abc_", TestName = "When_One_Of_Tags_Is_Screened")]
        [TestCase(@"\\_abc_", ExpectedResult = @"\<em>abc</em>", TestName = "When_Slash_Is_Screened")]
        [TestCase(@"\ _abc_", ExpectedResult = @"\ <em>abc</em>", TestName = "When_Slash_Do_Not_Screen")]
        public string Render_ConsiderScreening(string input)
        {
            return md.Render(input);
        }

        [TestCase("__a_b_c__", ExpectedResult = "<strong>a<em>b</em>c</strong>")]
        public string Render_DoubleUnderlineWorksInSingle(string input)
        {
            return md.Render(input);
        }

        [TestCase("_a__b__c_", ExpectedResult = "<em>a__b__c</em>")]
        public string Render_SingleUnderlineDoesNotWorkInDouble(string input)
        {
            return md.Render(input);
        }

        [TestCase("__a_b__c_", ExpectedResult = "__a_b__c_", TestName = "When_Italic_And_Bold_Tags")]
        [TestCase("__a# b__c", ExpectedResult = "__a# b__c", TestName = "When_Header_And_Bold_Tags")]
        [TestCase("xy_z[abc](vk.c_om)def", ExpectedResult = "xy_z<a href=\"vk.c_om\">abc</a>def",
            TestName = "When_Italic_Intersect_Link")]
        public string Render_ExcludeTagIntersections(string input)
        {
            return md.Render(input);
        }

        [TestCase("1as_ads_", ExpectedResult = "1as_ads_", TestName = "When_Tag_Inside_Word_With_Digit")]
        [TestCase("as_ads ab_cd", ExpectedResult = "as_ads ab_cd", TestName = "When_Tags_Inside_Different_Words")]
        [TestCase("____", ExpectedResult = "____", TestName = "When_Empty_String_Between_Tags")]
        [TestCase("_ abc_", ExpectedResult = "_ abc_", TestName = "When_Whitespace_After_Open_Tag")]
        [TestCase("_abc _", ExpectedResult = "_abc _", TestName = "When_Whitespace_Before_Close_Tag")]
        [TestCase("_ab\nc_", ExpectedResult = "_ab\nc_", TestName = "When_Tags_In_Different_Paragraphs")]
        public string Render_ExcludeTags(string input)
        {
            return md.Render(input);
        }

        [Test]
        public void IsAlgorithmFast()
        {
            GetElapsedTimeInTicks("# __t_es_t__");
            var firstStr = "_abc_";
            var secondStr = "__a_b_cd__";
            var firstStrTime = GetElapsedTimeInTicks(firstStr);
            var secondStrTime = GetElapsedTimeInTicks(secondStr);
            var timeLinearDiff = secondStrTime / firstStrTime - (double) secondStr.Length / firstStr.Length;
            Assert.IsTrue(timeLinearDiff < 1);
        }

        private double GetElapsedTimeInTicks(string firstStr)
        {
            var sw = Stopwatch.StartNew();
            md.Render(firstStr);
            return sw.Elapsed.Ticks;
        }
    }
}