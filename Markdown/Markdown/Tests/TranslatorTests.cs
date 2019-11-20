using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class TranslatorTests
    {
        private static string BuildRepeatedStringFromPattern(string pattern, int timesCount)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < timesCount; i++)
                builder.Append(pattern);
            return builder.ToString();
        }

        [TestCase("", TestName = "empty string")]
        [TestCase("abc", TestName = "raw text")]
        [TestCase("_abc", TestName = "not closed opening underscore at the beginning")]
        [TestCase("__abc", TestName = "not closed opening double underscore at the beginning")]
        [TestCase("___abc", TestName = "not closed opening triple underscore at the beginning")]
        [TestCase("abc_", TestName = "not closed opening underscore at the end")]
        [TestCase("abc__", TestName = "not closed opening double underscore at the end")]
        [TestCase("abc___", TestName = "not closed opening triple underscore at the end")]
        [TestCase("a_bc", TestName = "not closed opening underscore in the middle")]
        [TestCase("a__bc", TestName = "not closed opening double underscore in the middle")]
        [TestCase("a___bc", TestName = "not closed opening triple underscore in the middle")]
        [TestCase("_abc _", TestName = "space before closing underscore")]
        [TestCase("__abc __", TestName = "space before closing double underscore")]
        [TestCase("_12_3", TestName = "closing underscore between digits")]
        [TestCase("__12__3", TestName = "closing double underscore between digits")]
        [TestCase("1_23_", TestName = "opening underscore between digits")]
        [TestCase("1__23__", TestName = "opening double underscore between digits")]
        [TestCase("_____", TestName = "multiple underscores with no text")]
        [TestCase("_abc__", TestName = "text starting with underscore and finishing with double underscore")]
        [TestCase("__abc_", TestName = "text starting with double underscore and finishing with underscore")]
        public void Translate_ReturnsRawText_On(string text)
        {
            Translator.Translate(text).Should().Be(text);
        }

        [TestCase("_abc_", "<em>abc</em>", TestName = "all string is between underscores")]
        [TestCase("__abc__", "<strong>abc</strong>", TestName = "all string is between double underscores")]
        [TestCase("ab_cde_efg_kl_", "ab<em>cde</em>efg<em>kl</em>", TestName = "multiple underscores")]
        [TestCase("ab__cde__efg__kl__", "ab<strong>cde</strong>efg<strong>kl</strong>", TestName =
            "multiple double underscores")]
        [TestCase("___abc___", "<strong>_abc</strong>_", TestName = "triple underscores")]
        public void Translate_ReturnsCorrectString_OnSingleTag(string text, string expected)
        {
            Translator.Translate(text).Should().Be(expected);
        }

        [TestCase("__abc_d_f__", "<strong>abc<em>d</em>f</strong>", TestName =
            "underscores inside double underscores in the middle")]
        [TestCase("___abc_df__", "<strong><em>abc</em>df</strong>", TestName =
            "underscores inside double underscores at the beginning")]
        [TestCase("__abc_df___", "<strong>abc_df</strong>_", TestName =
            "underscores inside double underscores in the end")]
        [TestCase("_abc__d__f_", "<em>abc__d__f</em>", TestName =
            "double underscores inside underscores in the middle")]
        [TestCase("___abc__df_", "<strong>_abc</strong>df_", TestName =
            "double underscores inside underscores at the beginning")]
        [TestCase("_abc__df___", "<em>abc__df__</em>", TestName = "double underscores inside underscores in the end")]
        [TestCase("_a1_2b_", "<em>a1_2b</em>", TestName =
            "opening underscore between digits that are inside underscores")]
        [TestCase("__a1_2b__", "<strong>a1_2b</strong>", TestName =
            "opening underscore between digits that are inside double underscores")]
        public void Translate_ReturnsCorrectString_OnFewTagsTogether(string text, string expected)
        {
            Translator.Translate(text).Should().Be(expected);
        }

        [TestCase(@"\_abc_", "_abc_", TestName = "single backslash before opening underscore")]
        [TestCase(@"_abc\_", "_abc_", TestName = "single backslash before closing underscore")]
        [TestCase(@"\\_abc_", @"\<em>abc</em>", TestName = "double backslashes before opening underscore")]
        [TestCase(@"_abc\\_", @"<em>abc\</em>", TestName = "double backslashes before closing underscore")]
        [TestCase(@"\__abc__", "__abc__", TestName = "single backslash before opening double underscore")]
        [TestCase(@"__abc\__", "__abc__", TestName = "single backslash before closing double underscore")]
        [TestCase(@"\\\\", @"\\", TestName = "even number of backslashes")]
        [TestCase(@"\\\", @"\\", TestName = "odd number of backslashes")]
        public void Translate_ReturnsCorrectString_OnTextWithBackslashes(string text, string expected)
        {
            Translator.Translate(text).Should().Be(expected);
        }


        [Test]
        public void Translate_AlgorithmicComplexityIsCloseToLinear()
        {
            GetTranslateTime(100);
            var firstResult = GetTranslateTime(100);
            var secondResult = GetTranslateTime(200);
            var thirdResult = GetTranslateTime(400);
            ((thirdResult / secondResult) / (secondResult / firstResult)).Should().BeLessThan(2);
        }

        private long GetTranslateTime(int unitsNumber)
        {
            var stopWatch = new Stopwatch();
            var text = BuildRepeatedStringFromPattern("_a_ ", unitsNumber);
            stopWatch.Start();
            Translator.Translate(text);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }
    }
}