using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown.Models.Converters;
using Markdown.Models.ConvertOptions;
using Markdown.Models.ConvertOptions.UnionRules;
using Markdown.Models.Renders;
using Markdown.Models.Syntax;
using Markdown.Models.Tags.HtmlTags;
using Markdown.Models.Tags.MdTags;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTests
    {
        private Md render;

        [SetUp]
        public void SetUp()
        {
            var convertRules = ConvertingRulesFactory.GetAllRules();
            var tags = convertRules.Select(rule => rule.From).ToList();
            var options = new ConvertingOptions(
                convertRules, new[] { new UnmarkedListUnion() }, new NewParagraph());
            var toHtmlConverter = new MdToHtmlConverter(options);
            render = new Md(new Syntax(tags, new Escaped()), toHtmlConverter);
        }

        [TestCase(null, TestName = "TextIsNull")]
        public void Render_ShouldThrowArgumentNullException(string markdownText)
        {
            Assert.Throws<ArgumentNullException>(() => render.Render(markdownText));
        }

        [TestCase("#header", "<h1>header</h1>", TestName = "HeaderInText")]
        [TestCase("a _em text_ a", "a <em>em text</em> a", TestName = "OneTokenInText")]
        [TestCase("__em _in_ str__", "<strong>em <em>in</em> str</strong>", TestName = "PossibleNesting")]
        [TestCase("#header __str__", "<h1>header <strong>str</strong></h1>", TestName = "NestingInHeader")]
        [TestCase("_beg_inning", "<em>beg</em>inning", TestName = "InWordBeginning")]
        [TestCase("mi_ddl_e", "mi<em>ddl</em>e", TestName = "InMiddleOfWord")]
        [TestCase("e_nd_", "e<em>nd</em>", TestName = "InWordEnding")]
        [TestCase("+list element", "<li>list element</li>")]
        public void Render_ShouldRenderTags(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("", "", TestName = "EmptyWhenEmptyText")]
        [TestCase("__", "__", TestName = "EmptyTokenBetweenUnderscore")]
        [TestCase("____", "____", TestName = "EmptyTokenBetweenDoubleUnderscore")]
        [TestCase("text without md", "text without md", TestName = "NoMdTags")]
        [TestCase("_abc __ghi_ def__ jkl", "_abc __ghi_ def__ jkl", TestName = "IntersectingInParagraph")]
        [TestCase("__Non paired_", "__Non paired_", TestName = "NonPaired")]
        [TestCase("_ space after opening_", "_ space after opening_", TestName = "SpaceAfterOpening")]
        [TestCase("_space before closing _", "_space before closing _", TestName = "SpaceBeforeClosing")]
        [TestCase("_str __in__ em_", "<em>str __in__ em</em>", TestName = "NotPermittedNesting")]
        [TestCase("diff_erent wor_ds", "diff_erent wor_ds", TestName = "InDifferentWords")]
        [TestCase("digits_1_23", "digits_1_23", TestName = "InWordWithDigits")]
        public void Render_ShouldNotRenderTags(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase(@"\non \escaping", @"\non \escaping", TestName = "RemoveOnlyTagsEscaping")]
        [TestCase(@"\# escaping header", @"# escaping header", TestName = "EscapeHeader")]
        [TestCase(@"\\_escaping symbol_", @"\<em>escaping symbol</em>", TestName = "EscapeEscaped")]
        [TestCase(@"\_escaping tag_", @"_escaping tag_", TestName = "EscapeUnderscore")]
        [TestCase(@"\__difficult_", @"_<em>difficult</em>", TestName = "EscapeTheShortestTag")]
        public void Render_ShouldRenderEscapedSymbols(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("_foo bar_ __baz_baz_baz__")]
        public void Render_WorkLinear(string rawText)
        {
            const double someConstant = 4.5;
            for (var i = 0; i < 10; i++)
                render.Render(rawText);

            var averageTimeRawText = GetAverageWorkTime(rawText, 100);

            var oneThousandRawText = Concat(rawText, rawText, 1000);
            var averageTimeOneThousandRawText = GetAverageWorkTime(oneThousandRawText, 100);

            var twoThousandRawText = Concat(oneThousandRawText, rawText, 1000);
            var averageTimeTwoThousandRawText = GetAverageWorkTime(twoThousandRawText, 100);

            averageTimeOneThousandRawText.Should()
                .BeLessOrEqualTo(someConstant * 1000 * averageTimeRawText);
            averageTimeTwoThousandRawText.Should()
                .BeLessOrEqualTo(someConstant * 2 * averageTimeOneThousandRawText);
        }

        private static string Concat(string source, string other, int count)
        {
            for (var i = 0; i < count; i++)
                source += $" {other}";
            return source;
        }

        private double GetAverageWorkTime(string rawText, int count)
        {
            var times = new List<long>();
            var watch = new Stopwatch();
            for (var i = 0; i < count; i++)
            {
                watch.Start();
                render.Render(rawText);
                watch.Stop();
                times.Add(watch.ElapsedTicks);
                watch.Reset();
            }

            return times.Average();
        }
    }
}
