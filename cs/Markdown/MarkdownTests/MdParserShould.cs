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
                    new TagEvent(TagSide.Opening, TagKind.Header, "#"),
                    new TagEvent(TagSide.None, TagKind.PlainText, "header with several words"),
                    new TagEvent(TagSide.None, TagKind.PlainText, "\n")
                }).SetName("simple header");
            yield return new TestCaseData(
                "_simple single underline_",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Opening, TagKind.SingleUnderline, "_"),
                    new TagEvent(TagSide.None, TagKind.PlainText, "simple single underline"),
                    new TagEvent(TagSide.Closing, TagKind.SingleUnderline, "_")
                }).SetName("simple single underline");
            yield return new TestCaseData(
                "__simple double underline__",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Opening, TagKind.DoubleUnderline, "__"),
                    new TagEvent(TagSide.None, TagKind.PlainText, "simple double underline"),
                    new TagEvent(TagSide.Closing, TagKind.DoubleUnderline, "\n")
                }).SetName("simple double underline");
        }
    }
}
