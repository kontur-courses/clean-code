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
                    new TagEvent(Side.Left, Mark.Header, "#"),
                    new TagEvent(Side.None, Mark.Text, "header with several words"),
                    new TagEvent(Side.Right, Mark.Header, "\n"),
                    new TagEvent(Side.None, Mark.Text, "")
                }).SetName("simple header");
            yield return new TestCaseData(
                "_simple single underline_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.OneLine, "_"),
                    new TagEvent(Side.None, Mark.Text, "simple single underline"),
                    new TagEvent(Side.Right, Mark.OneLine, "_"),
                    new TagEvent(Side.None, Mark.Text, "")
                }).SetName("simple single underline");
            yield return new TestCaseData(
                "__simple double underline__",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.TwoLines, "__"),
                    new TagEvent(Side.None, Mark.Text, "simple double underline"),
                    new TagEvent(Side.Right, Mark.TwoLines, "__"),
                    new TagEvent(Side.None, Mark.Text, "")
                }).SetName("simple double underline");
            yield return new TestCaseData(
                "just text",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "just text"),
                    new TagEvent(Side.None, Mark.Text, "")
                }).SetName("two words without tags");
            yield return new TestCaseData(
                "First line.\nSecond line.",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "First line."),
                    new TagEvent(Side.None, Mark.Text, "\n"),
                    new TagEvent(Side.None, Mark.Text, "Second line."),
                    new TagEvent(Side.None, Mark.Text, "")
                }).SetName("two sentences separated by new line symbol");
            yield return new TestCaseData(
                "#Header without new line",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Header, "#"),
                    new TagEvent(Side.None, Mark.Text, "Header without new line"),
                    new TagEvent(Side.Right, Mark.Header, ""),
                }).SetName("opened header without new line symbol");
        }
    }
}
