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
                    new TagEvent(Side.Opening, Tag.Header, "#"),
                    new TagEvent(Side.None, Tag.Text, "header with several words"),
                    new TagEvent(Side.None, Tag.Text, "\n")
                }).SetName("simple header");
            yield return new TestCaseData(
                "_simple single underline_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Opening, Tag.OneLine, "_"),
                    new TagEvent(Side.None, Tag.Text, "simple single underline"),
                    new TagEvent(Side.Closing, Tag.OneLine, "_")
                }).SetName("simple single underline");
            yield return new TestCaseData(
                "__simple double underline__",
                new List<TagEvent>
                {
                    new TagEvent(Side.Opening, Tag.TwoLines, "__"),
                    new TagEvent(Side.None, Tag.Text, "simple double underline"),
                    new TagEvent(Side.Closing, Tag.TwoLines, "\n")
                }).SetName("simple double underline");
        }
    }
}
