using System;
using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using Markdown.Tag_Classes;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MdTaginizerShould
    {
        private HashSet<char> _tagSymbols = new HashSet<char>() { '#', '_', '\n'};

        [TestCaseSource(nameof(SimpleCasesTestData))]
        [TestCaseSource(nameof(DifferentWordsUnderlining))]
        [TestCaseSource(nameof(WordsPartsUnderlining))]
        public void ProduceTagEventsCorrectly(string input, List<TagEvent> expectedTagEvents)
        {
            var taginizer = new MdTaginizer(input, _tagSymbols);

            var tagEvents = taginizer.GetTagEvents();

            tagEvents.Should().BeEquivalentTo(expectedTagEvents);
        }

        public static IEnumerable<TestCaseData> WordsPartsUnderlining()
        {
            yield return new TestCaseData(
                "_wo_rd",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "wo"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "rd")
                }).SetName("underline word beginnig");
            yield return new TestCaseData(
                "wo_rd_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "wo"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "rd"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),

                }).SetName("underline word ending");
            yield return new TestCaseData(
                "wo_r_d",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "wo"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "r"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "d"),
                }).SetName("underline word middle");
        }

        public static IEnumerable<TestCaseData> DifferentWordsUnderlining()
        {
            yield return new TestCaseData(
                "fi_st wo_rd",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fi"),
                    new TagEvent(Side.None, Mark.Text, "_"),
                    new TagEvent(Side.None, Mark.Text, "st wo"),
                    new TagEvent(Side.None, Mark.Text, "_"),
                    new TagEvent(Side.None, Mark.Text, "rd")
                }).SetName("single underliners in different words");
            yield return new TestCaseData(
                "fi__st wo__rd",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fi"),
                    new TagEvent(Side.None, Mark.Text, "__"),
                    new TagEvent(Side.None, Mark.Text, "st wo"),
                    new TagEvent(Side.None, Mark.Text, "__"),
                    new TagEvent(Side.None, Mark.Text, "rd")
                }).SetName("double underliners in different words");
        }

        public static IEnumerable<TestCaseData> SimpleCasesTestData()
        {
            yield return new TestCaseData(
                "#header with several words\n",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Header, "#"),
                    new TagEvent(Side.None, Mark.Text, "header with several words"),
                    new TagEvent(Side.Right, Mark.Header, "\n")
                }).SetName("simple header");
            yield return new TestCaseData(
                "_simple single underline_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "simple single underline"),
                    new TagEvent(Side.Right, Mark.Underliner, "_")
                }).SetName("simple single underline");
            yield return new TestCaseData(
                "__simple double underline__",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.DoubleUnderliner, "__"),
                    new TagEvent(Side.None, Mark.Text, "simple double underline"),
                    new TagEvent(Side.Right, Mark.DoubleUnderliner, "__"),
                }).SetName("simple double underline");
            yield return new TestCaseData(
                "just text",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "just text"),
                }).SetName("two words without tags");
            yield return new TestCaseData(
                "First line.\nSecond line.",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "First line."),
                    new TagEvent(Side.Right, Mark.Header, "\n"),
                    new TagEvent(Side.None, Mark.Text, "Second line."),
                }).SetName("two sentences separated by new line symbol");
            yield return new TestCaseData(
                "#Header without new line",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Header, "#"),
                    new TagEvent(Side.None, Mark.Text, "Header without new line"),
                }).SetName("opened header without new line symbol");
        }
    }
}
