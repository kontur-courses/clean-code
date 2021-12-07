using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class EscapingTagParserShould
    {
        [TestCaseSource(nameof(EscapeTagCases))]
        public void ParseEscapeTagCorrectlyWhen(string input, List<TagEvent> expectedTags)
        {
            var tagEvents = new Taginizer(input).Taginize();
            var parser = new EscapingTagParser(tagEvents);

            var escapedTags = parser.Parse();

            escapedTags.Should().BeEquivalentTo(expectedTags);
        }

        private static IEnumerable<TestCaseData> EscapeTagCases()
        {
            yield return new TestCaseData(
                "\\hello\\",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("simple word escaped on both sides");
            yield return new TestCaseData(
                "\\\\",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("escape symbol is escaped");
            yield return new TestCaseData(
                "\\\\\\",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("odd number of escape symbols are given");
            yield return new TestCaseData(
                "\\\\\\\\\\\\",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\\"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("even number (bigger than two) of escape symbols are given");
            yield return new TestCaseData(
                "\\_",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("single underline is escaped");
            yield return new TestCaseData(
                "\\\n",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("new line symbol is escaped");
            yield return new TestCaseData(
                "\\#",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }
            ).SetName("escape tag header symbol");
        }
    }
}
