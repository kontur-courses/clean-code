using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown.Models;
using Markdown.Models.Converters;
using Markdown.Models.ConvertOptions;
using Markdown.Models.ConvertOptions.ConvertRules;
using Markdown.Models.ConvertOptions.UnionRules;
using Markdown.Models.Renders;
using Markdown.Models.Syntax;
using Markdown.Models.Tags.MdTags;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Md_RenderTests
    {
        private Md render;

        [SetUp]
        public void SetUp()
        {
            var convertRules = GetAllRules();
            var tags = convertRules.Select(rule => rule.From).ToList();
            var options = new ConvertingOptions(convertRules,
                new[] { new UnmarkedListUnion() }, new NewLineToNewParagraph());
            var toHtmlConverter = new MdToHtmlConverter(options);
            var reader = new TokenReader(new Syntax(tags, new Escaped()));
            render = new Md(reader, toHtmlConverter);
        }

        [TestCase(null, TestName = "TextIsNull")]
        public void ShouldThrowArgumentNullException(string markdownText)
        {
            Assert.Throws<ArgumentNullException>(() => render.Render(markdownText));
        }

        #region escaping

        [TestCase(@"\_aaa_", @"_aaa_", TestName = "Underscore")]
        [TestCase(@"\_\_aaa\_\_", @"__aaa__", TestName = "DoubleUnderscore")]
        [TestCase(@"\# aaa", @"# aaa", TestName = "Sharp")]
        [TestCase(@"\+ aaa", @"+ aaa", TestName = "Plus")]
        [TestCase(@"\\_aaa_", @"\<em>aaa</em>", TestName = "Escape")]
        public void AllTagsAvailableForEscaping(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase(@"\aaa", @"\aaa", TestName = "WordBeginning")]
        [TestCase(@"aa\aa", @"aa\aa", TestName = "MiddleOfWord")]
        [TestCase(@"aaa\ ", @"aaa\ ", TestName = "WordEnding")]
        public void OnlyTagsAvailableForEscaping(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase(@"\__aaa_", @"_<em>aaa</em>", TestName = "EscapeTheShortestTag")]
        public void EscapingWorksWithTheShortestTag(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        #endregion

        #region underscore

        [TestCase("_aa_a", "<em>aa</em>a", TestName = "Beginning")]
        [TestCase("a_aa_a", "a<em>aa</em>a", TestName = "MiddleOfWord")]
        [TestCase("a_aa_", "a<em>aa</em>", TestName = "Ending")]
        public void UnderscoreShouldHighlightWordPart(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("_12_aa", "_12_aa", TestName = "Beginning")]
        [TestCase("aa_1_23", "aa_1_23", TestName = "MiddleOfWord")]
        [TestCase("aa3_12_", "aa3_12_", TestName = "Ending")]
        public void UnderscoreShouldNotWorkingInWordWithDigits(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("__", "__", TestName = "EmptyTokenInside")]
        [TestCase("_ aaa_", "_ aaa_", TestName = "SpaceAfterOpening")]
        [TestCase("_aaa _", "_aaa _", TestName = "SpaceBeforeClosing")]
        [TestCase("_aa __bb_ aa__ bb", "_aa __bb_ aa__ bb",
            TestName = "IntersectsWithDoubleUnderscore")]
        [TestCase("_a_b_c", "<em>a</em>b_c", TestName = "HaveNoPair")]
        public void UnderscoreCanNotBeHighlightingTag(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("__aa_bb_aa__", "<strong>aa<em>bb</em>aa</strong>")]
        public void DoubleUnderscoreCanBeInsideUnderscore(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("aa_aa bb_bb", "aa_aa bb_bb")]
        public void UnderscoreCanNotBeInDifferentWords(
            string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        #endregion 

        #region double underscore

        [TestCase("__aa__a", "<strong>aa</strong>a", TestName = "Beginning")]
        [TestCase("a__aa__a", "a<strong>aa</strong>a", TestName = "MiddleOfWord")]
        [TestCase("a__aa__", "a<strong>aa</strong>", TestName = "Ending")]
        public void DoubleUnderscoreShouldHighlightWordPart(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("__12__aa", "__12__aa", TestName = "Beginning")]
        [TestCase("aa__1__23", "aa__1__23", TestName = "MiddleOfWord")]
        [TestCase("aa3__12__", "aa3__12__", TestName = "Ending")]
        public void DoubleUnderscoreShouldNotWorkingInWordWithDigits(
            string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("____", "____", TestName = "EmptyTokenInside")]
        [TestCase("__ aaa__", "__ aaa__", TestName = "SpaceAfterOpening")]
        [TestCase("__aaa __", "__aaa __", TestName = "SpaceBeforeClosing")]
        [TestCase("__aa _bb__ aa_ bb", "__aa _bb__ aa_ bb", TestName = "IntersectsWithUnderscore")]
        [TestCase("__a__b__c", "<strong>a</strong>b__c", TestName = "HaveNoPair")]
        public void DoubleUnderscoreCanNotBeHighlightingTag(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("_aa__bb__aa_", "<em>aa__bb__aa</em>")]
        public void UnderscoreCanNotBeInsideDoubleUnderscore(
            string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("aa__aa bb__bb", "aa__aa bb__bb")]
        public void DoubleUnderscoreCanNotBeInDifferentWords(
            string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        #endregion

        #region sharp       

        [TestCase(" # aaa", " # aaa", TestName = "WithWhiteSpacesAtParagraphBeginning")]
        [TestCase("aaa # bbb", "aaa # bbb", TestName = "InMiddleOfParagraph")]
        public void SharpDoesNotWork_WhenNotAtFirstPosition(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("# aa _bb_", "<h1>aa <em>bb</em></h1>", TestName = "Underscore")]
        [TestCase("# aa __bb__", "<h1>aa <strong>bb</strong></h1>", TestName = "DoubleUnderscore")]
        public void AllNonStartParagraphTagsCanBeInsideSharp(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase(@"# aa \_bb\_", "<h1>aa _bb_</h1>", TestName = "Escaping")]
        [TestCase(@"# a __b_c_b__", "<h1>a <strong>b<em>c</em>b</strong></h1>",
            TestName = "MultiplyNesting")]
        public void AllRulesWorkInsideSharp(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        #endregion

        #region plus

        [TestCase("+ a\n+ b\n+ c", "<ul><li>a</li><li>b</li><li>c</li></ul>")]
        public void ParagraphsWithPlusShouldBeCombinedAsUnmarkedList(
            string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase(" + aaa", " + aaa", TestName = "WithWhiteSpacesAtParagraphBeginning")]
        [TestCase("aaa + bbb", "aaa + bbb", TestName = "InMiddleOfParagraph")]
        public void PlusDoesNotWork_WhenNotAtFirstPosition(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("+ aa _bb_", "<ul><li>aa <em>bb</em></li></ul>", TestName = "Underscore")]
        [TestCase("+ aa __bb__", "<ul><li>aa <strong>bb</strong></li></ul>",
            TestName = "DoubleUnderscore")]
        public void AllNonStartParagraphTagsCanBeInsidePlus(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        #endregion

        [TestCase("", "", TestName = "EmptyText")]
        [TestCase("aa bb cc", "aa bb cc", TestName = "SeveralWords")]
        public void TextWithoutTagsShouldNotBeChanged(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("_ab_cd_e_", "<em>ab</em>cd<em>e</em>", TestName = "OnlyUnderscore")]
        [TestCase("a_e_b__c__d", "a<em>e</em>b<strong>c</strong>d", TestName = "DifferentTypes")]
        public void MultiplyTagsArePossibleInOneParagraph(string markdownText, string expected)
        {
            render.Render(markdownText).Should().Be(expected);
        }

        [TestCase("_foo bar_ __baz_baz_baz__")]
        public void ShouldWorkLinear(string rawText)
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

        private static List<IConvertRule> GetAllRules()
        {
            return new List<IConvertRule>()
            {
                new UnderscoreToEm(),
                new DoubleUnderscoreToStrong(),
                new SharpToH1(),
                new PlusToUnorderedListElement()
            };
        }
    }
}
