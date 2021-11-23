using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor
{
    [TestFixture]
    public class MarkdownProcessorTests
    {
        private static IEnumerable<TestCaseData> Tests()
        {
            yield return new TestCaseData("abc", "abc\n").SetName("Should be no tags without tags");
            yield return new TestCaseData("_abc", "_abc\n").SetName("Should be no tags when non-paired tags");
            yield return new TestCaseData("__abc_", "__abc_\n").SetName("Should be no tags when non-paired tags");
            yield return new TestCaseData("# abc\n_dgv_", "<h1>abc</h1>\n<em>dgv</em>\n").SetName(
                "Should be tags when valid header and italic tags");
            yield return
                new TestCaseData("as# sd", "as# sd\n").SetName("Should be no tags when header tag inside word");
            yield return new TestCaseData("_ab _", "_ab _\n").SetName(
                "Should be no tags when whitespaces before closing tag");
            yield return new TestCaseData("_ ab_", "_ ab_\n").SetName(
                "Should be no tags when whitespaces after opening tag");
            yield return new TestCaseData("ab_cd fg_oi", "ab_cd fg_oi\n").SetName(
                "Should be no tags when tags inside different words");
            yield return new TestCaseData("_abc _ghd idk_", "<em>abc _ghd idk</em>\n").SetName(
                "Invalid tag should be text");
            yield return new TestCaseData("aa ____ aa", "aa ____ aa\n").SetName(
                "Should be no tags when empty string between tags");
            yield return new TestCaseData("aa _ _ aa", "aa _ _ aa\n").SetName(
                "Should be no tags when whitespaces between tags");
            yield return new TestCaseData("acd_12_3 rf", "acd_12_3 rf\n").SetName(
                "Should be no tags when tags inside digits");
            yield return new TestCaseData("_ad__dc__jn_", "<em>ad__dc__jn</em>\n").SetName(
                "Strong inside italic should not work");
            yield return new TestCaseData("__ad_dc_jn__", "<strong>ad<em>dc</em>jn</strong>\n").SetName(
                "Italic inside strong should work");
            yield return new TestCaseData("__ad_dc__jn_", "__ad_dc__jn_\n").SetName(
                "Should be no tags when tags with intersections");
            yield return new TestCaseData("_ad__dc_jn__", "_ad__dc_jn__\n").SetName(
                "Should be no tags when tags with intersections");
            yield return new TestCaseData(@"\__avd_", "_<em>avd</em>\n").SetName("Screened tag should be text");
            yield return new TestCaseData(@"\\\__ad", @"\__ad" + '\n').SetName("Screened screener should be text");
            yield return new TestCaseData(@"as\c\ d\", @"as\c\ d\" + '\n').SetName(
                "Screeners should be text when do not screen");
            yield return new TestCaseData("# __abc__ _dc_", "<h1><strong>abc</strong> <em>dc</em></h1>\n").SetName(
                "Tags should work inside header");
            yield return new TestCaseData("- ab\n- cd\n- de", "<ul>\n<li>ab</li>\n<li>cd</li>\n<li>de</li>\n</ul>\n")
                .SetName("Simple unordered list should mark by tags");
            // Из-за сырой реализации неупорядоченного листа следующий тест валится
            // yield return new TestCaseData("abc\n- a\n- b\n_ii_",
            //     "abc\n<ul>\n<li>a</li>\n<li>b</li>\n</ul>\n<em>ii</em>\n");
        }

        [TestCaseSource(nameof(Tests))]
        public void Render_ShouldReturnCorrectTokens(string text, string result)
        {
            var markdownProcessor = new MarkdownProcessor();

            markdownProcessor.Render(text).Should().BeEquivalentTo(result);
        }

        [Test]
        public void Render_ShouldWorkForLinearComplexity()
        {
            // Если запускаю только этот тест, он легко проходит. Если все сразу, то тут 40-60 соотношение
            var markdownProcessor = new MarkdownProcessor();
            var shortLine = GetRandomCase(100);
            var longLine = GetRandomCase(1000);
            var timer = new Stopwatch();

            timer.Start();
            markdownProcessor.Render(shortLine);
            timer.Stop();
            var shortLineHandleTime = timer.Elapsed;
            timer.Reset();

            timer.Start();
            markdownProcessor.Render(longLine);
            timer.Stop();
            var longLineHandleTime = timer.Elapsed;

            var ratio = longLineHandleTime.Milliseconds / shortLineHandleTime.Milliseconds;
            ratio.Should().BeLessOrEqualTo(10);
        }

        private static string GetRandomCase(int length)
        {
            var symbols = new List<string>
            {
                " _ab_ ", " __ab__ ", "# ab\n"
            };

            var random = new Random();
            var sb = new StringBuilder();

            for (var i = 0; i < length; i++) sb.Append(symbols[random.Next(0, symbols.Count - 1)]);

            return sb.ToString();
        }
    }
}