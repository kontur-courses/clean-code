using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NUnit.Framework;
using FluentAssertions;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace Markdown
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md(new HtmlMark("__", "<strong>"), new HtmlMark("_", "<em>"), 
                new HtmlMark("~", "<sub>"), new HtmlMark("~~", "<s>"));
        }

        [TestCase("", TestName = "empty string")]
        [TestCase("abc", TestName = "raw text")]
        [TestCase("_abc", TestName = "not closed opening underscore at the beginning")]
        [TestCase("__abc", TestName = "not closed opening double underscore at the beginning")]
        [TestCase("~abc", TestName = "not closed opening single tilde at the beginning")]
        [TestCase("~~abc", TestName = "not closed opening double tildes at the beginning")]
        [TestCase("___abc", TestName = "not closed opening triple underscore at the beginning")]
        [TestCase("___abc", TestName = "not closed opening triple tildes at the beginning")]
        [TestCase("abc_", TestName = "not closed opening underscore at the end")]
        [TestCase("abc__", TestName = "not closed opening double underscore at the end")]
        [TestCase("abc~", TestName = "not closed opening tilde at the end")]
        [TestCase("abc~~", TestName = "not closed opening double tildes at the end")]
        [TestCase("abc___", TestName = "not closed opening triple underscore at the end")]
        [TestCase("abc~~~", TestName = "not closed opening triple tildes at the end")]
        [TestCase("a_bc", TestName = "not closed opening underscore in the middle")]
        [TestCase("a__bc", TestName = "not closed opening double underscore in the middle")]
        [TestCase("a___bc", TestName = "not closed opening triple underscore in the middle")]
        [TestCase("a~bc", TestName = "not closed opening tilde in the middle")]
        [TestCase("a~~bc", TestName = "not closed opening double tildes in the middle")]
        [TestCase("a~~~bc", TestName = "not closed opening triple tildes in the middle")]
        [TestCase("_abc _", TestName = "space before closing underscore")]
        [TestCase("__abc __", TestName = "space before closing double underscore")]
        [TestCase("~abc ~", TestName = "space before closing tilde")]
        [TestCase("~~abc ~~", TestName = "space before closing double tildes")]
        [TestCase("_12_3", TestName = "closing underscore between digits")]
        [TestCase("__12__3", TestName = "closing double underscore between digits")]
        [TestCase("~12~3", TestName = "closing tilde between digits")]
        [TestCase("~~12~~3", TestName = "closing double tildes between digits")]
        [TestCase("1_23_", TestName = "opening underscore between digits")]
        [TestCase("1__23__", TestName = "opening double underscore between digits")]
        [TestCase("1~23~", TestName = "opening tilde between digits")]
        [TestCase("1~~23~~", TestName = "opening double tilde between digits")]
        [TestCase("_____", TestName = "multiple underscores with no text")]
        [TestCase("~~~~~", TestName = "multiple tildes with no text")]
        [TestCase("_abc__", TestName = "text starting with underscore and finishing with double underscore")]
        [TestCase("__abc_", TestName = "text starting with double underscore and finishing with underscore")]
        [TestCase("_abc__", TestName = "text starting with tilde and finishing with double tilde")]
        [TestCase("__abc_", TestName = "text starting with double tildes and finishing with tilde")]
        public void Render_ReturnsRawText_On(string text)
        {
            md.Render(text).Should().Be(text);
        }

        [TestCase("_abc_", "<em>abc</em>", TestName = "all string is between underscores")]
        [TestCase("__abc__", "<strong>abc</strong>", TestName = "all string is between double underscores")]
        [TestCase("~abc~", "<sub>abc</sub>", TestName = "all string is between tildes")]
        [TestCase("~~abc~~", "<s>abc</s>", TestName = "all string is between double tildes")]
        [TestCase("ab_cde_efg_kl_", "ab<em>cde</em>efg<em>kl</em>", TestName = "multiple underscores")]
        [TestCase("ab__cde__efg__kl__", "ab<strong>cde</strong>efg<strong>kl</strong>", TestName = "multiple double underscores")]
        [TestCase("ab~cde~efg~kl~", "ab<sub>cde</sub>efg<sub>kl</sub>", TestName = "multiple tildes")]
        [TestCase("ab~~cde~~efg~~kl~~", "ab<s>cde</s>efg<s>kl</s>", TestName = "multiple double tildes")]
        [TestCase("___abc___", "<strong>_abc</strong>_", TestName = "triple underscores")]
        [TestCase("~~~abc~~~", "<s>~abc</s>~", TestName = "triple tildes")]
        [TestCase("__abc_d_f__", "<strong>abc<em>d</em>f</strong>", TestName = "underscores inside double underscores in the middle")]
        [TestCase("___abc_df__", "<strong><em>abc</em>df</strong>", TestName = "underscores inside double underscores at the beginning")]
        [TestCase("__abc_df___", "<strong>abc_df</strong>_", TestName = "underscores inside double underscores in the end")]
        [TestCase("_abc__d__f_", "<em>abc<strong>d</strong>f</em>", TestName = "double underscores inside underscores in the middle")]
        [TestCase("___abc__df_", "<strong>_abc</strong>df_", TestName = "double underscores inside underscores at the beginning")]
        [TestCase("_abc__df___", "<em>abc<strong>df</strong></em>", TestName = "double underscores inside underscores in the end")]
        [TestCase("_a1_3b_", "<em>a1_3b</em>", TestName = "opening underscore between digits that are inside underscores")]
        [TestCase("__a1_3b__", "<strong>a1_3b</strong>", TestName = "opening underscore between digits that are inside double underscores")]
        [TestCase(@"\_abc_", "_abc_", TestName = "single backslash before opening underscore")]
        [TestCase(@"_abc\_", "_abc_", TestName = "single backslash before closing underscore")]
        [TestCase(@"\~abc~", "~abc~", TestName = "single backslash before opening tilde")]
        [TestCase(@"~abc\~", "~abc~", TestName = "single backslash before closing tilde")]
        [TestCase(@"\\_abc_", @"\<em>abc</em>", TestName = "double backslashes before opening underscore")]
        [TestCase(@"_abc\\_", @"<em>abc\</em>", TestName = "double backslashes before closing underscore")]
        [TestCase(@"\__abc__", "__abc__", TestName = "single backslash before opening double underscore")]
        [TestCase(@"__abc\__", "__abc__", TestName = "single backslash before closing double underscore")]
        [TestCase(@"\~~abc~~", "~~abc~~", TestName = "single backslash before opening double tilde")]
        [TestCase(@"~~abc\~~", "~~abc~~", TestName = "single backslash before closing double tilde")]
        [TestCase(@"\\\\\\\\", @"\\\\", TestName = "even number of backslashes")]
        [TestCase(@"\\\\\\\\\", @"\\\\\", TestName = "odd number of backslashes")]
        public void Render_ReturnsCorrectString_On(string text, string expected)
        {
            md.Render(text).Should().Be(expected);
        }

        [Test]
        public void Render_WorksFast_OnNotNested1000MultipleTags()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 1000; i++)
                builder.Append("_k_");
            var text = builder.ToString();
            Action action = () => md.Render(text);
            action.ExecutionTime().ShouldNotExceed(1000.Milliseconds());
        }

        [Test]
        public void Render_WorksFast_OnNested1000MultipleTags()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 1000; i++)
            {
                builder.Append("_~k~_");
            }

            var text = builder.ToString();
            Action action = () => md.Render(text);
            action.ExecutionTime().ShouldNotExceed(1000.Milliseconds());
        }

        [Test]
        public void Render_WorksFast_ORawStringWith10000Chars()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < 10000; i++)
                builder.Append("A");
            var text = builder.ToString();
            Action action = () => md.Render(text);
            action.ExecutionTime().ShouldNotExceed(1000.Milliseconds());
        }

        [Test]
        public void Render_ReturnsCorrectString_OnMultipleNestedTags()
        {
            var text = "_a_b__c__~d~e~~f~~g";
            var result = md.Render(text);
            Approvals.Verify(result);
        }

        [Test]
        public void Render_ReturnsCorrectString_OnMultipleNotNestedTags()
        {
            var text = "_a__b~c~~d~~~__k~l~_";
            var result = md.Render(text);
            Approvals.Verify(result);
        }

        [Test]
        public void Render_AlgorithmicComplexityIsCloseToLinear()
        {
            GetRenderTime(100);

            var firstResult = GetRenderTime(100);
            var secondResult = GetRenderTime(200);
            var thirdResult = GetRenderTime(400);
            Assert.True((thirdResult / secondResult)/(secondResult / firstResult) <= 2);
        }

        private long GetRenderTime(int unitsNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            var builder = new StringBuilder();
            for (var i = 0; i < unitsNumber; i++)
                builder.Append("_k_");
            var text = builder.ToString();
            stopWatch.Start();
            md.Render(text);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }
    }
}
