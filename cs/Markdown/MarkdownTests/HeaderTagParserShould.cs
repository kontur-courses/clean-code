using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class HeaderTagParserShould
    {
        [TestCaseSource(nameof(HeaderTestCases))]
        public void ParseHeadersCorrectlyWhen(string input, List<TagEvent> expectedTagEvents)
        {
            var tagEvents = new Taginizer(input).Taginize();
            tagEvents = new EscapingTagParser(tagEvents).Parse();
            var parsedHeaders = new HeaderTagParser(tagEvents).Parse();

            parsedHeaders.Should().BeEquivalentTo(expectedTagEvents);
        }

        private static IEnumerable<TestCaseData> HeaderTestCases()
        {
            yield return new TestCaseData(
                "# hello!",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "hello!"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header and text without newline are given");
            yield return new TestCaseData(
                "## hello!",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "hello!"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("double header and text without newline are given");
            yield return new TestCaseData(
                "# hello!\n",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "hello!"),
                    new TagEvent(TagSide.Right, TagName.NewLine, "\n"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header and text with newline are given");
            yield return new TestCaseData(
                "\n\n#",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header after double new line are given");
            yield return new TestCaseData(
                "#\n#",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.Right, TagName.NewLine, "\n"),
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header after headers pair is given");
            yield return new TestCaseData(
                "word#\n",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, "word"),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("headers pair after word is given");
            yield return new TestCaseData(
                "word#\n#",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, "word"),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header after headers pair after word is given");
            yield return new TestCaseData(
                "\\#\n",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, ""),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Word, "\n"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header is escaped in headers pair");
        }
    }
}
