using Markdown.Tag_Classes;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MdParserShould
    {
        [TestCaseSource(nameof(SimpleCasesTestData))]
        public void ParseSimpleCasesCorrect(string rawInput, List<TagEvent> parsedTags)
        {
            var parser = new MdParser();

            var parsingResult = parser.Parse(rawInput);

            parsingResult.Should().BeEquivalentTo(parsedTags);
        }

        public static IEnumerable<TestCaseData> SimpleCasesTestData()
        {
            yield return new TestCaseData(
                "#header with several words\n",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Tag.Header, "#"),
                    new TagEvent(Side.None, Tag.Text, "header with several words"),
                    new TagEvent(Side.Right, Tag.Header, "\n"),
                    new TagEvent(Side.None, Tag.Text, "")
                }).SetName("simple header");
            yield return new TestCaseData(
                "_simple single underline_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Tag.OneLine, "_"),
                    new TagEvent(Side.None, Tag.Text, "simple single underline"),
                    new TagEvent(Side.Right, Tag.OneLine, "_"),
                    new TagEvent(Side.None, Tag.Text, "")
                }).SetName("simple single underline");
            yield return new TestCaseData(
                "__simple double underline__",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Tag.TwoLines, "__"),
                    new TagEvent(Side.None, Tag.Text, "simple double underline"),
                    new TagEvent(Side.Right, Tag.TwoLines, "__"),
                    new TagEvent(Side.None, Tag.Text, "")
                }).SetName("simple double underline");
            yield return new TestCaseData(
                "just text",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Tag.Text, "just text"),
                    new TagEvent(Side.None, Tag.Text, "")
                }).SetName("two words without tags");
            yield return new TestCaseData(
                "First line.\nSecond line.",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Tag.Text, "First line."),
                    new TagEvent(Side.None, Tag.Text, "\n"),
                    new TagEvent(Side.None, Tag.Text, "Second line."),
                    new TagEvent(Side.None, Tag.Text, "")
                }).SetName("two sentences separated by new line symbol");
            yield return new TestCaseData(
                "#Header without new line",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Tag.Header, "#"),
                    new TagEvent(Side.None, Tag.Text, "Header without new line"),
                    new TagEvent(Side.Right, Tag.Header, ""),
                }).SetName("opened header without new line symbol");
        }
    }
}
