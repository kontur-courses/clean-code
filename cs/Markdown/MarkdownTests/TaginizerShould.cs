using System.Collections.Generic;
using FluentAssertions;
using Markdown.TagEvents;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class TaginizerShould
    {
        [TestCaseSource(nameof(SimpleCases))]
        public void TaginizeCorrectlyWhen(string input, List<TagEvent> expectedTagEvents)
        {
            var taginizer = new Taginizer(input);

            var tagEvents = taginizer.Taginize();

            tagEvents.Should().BeEquivalentTo(expectedTagEvents);
        }

        public static IEnumerable<TestCaseData> SimpleCases()
        {
            yield return new TestCaseData(
                "# # # # ",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "#"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("several header symbols are given");
            yield return new TestCaseData(
                "peo5ple",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Word, "peo"),
                    new TagEvent(TagSide.None, TagName.Number, "5"),
                    new TagEvent(TagSide.None, TagName.Word, "ple"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("two words are separated by digit");
            yield return new TestCaseData(
                "___",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("three underliners are given");
            yield return new TestCaseData(
                "    _",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Whitespace, "    "),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("several whitespaces ended with single underline");
            yield return new TestCaseData(
                "55.123",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Number, "55"),
                    new TagEvent(TagSide.None, TagName.Word, "."),
                    new TagEvent(TagSide.None, TagName.Number, "123"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("two digits are separated by dot");
            yield return new TestCaseData(
                "\\# ",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.Escape, "\\"),
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("header is escaped");
            yield return new TestCaseData(
                "_______",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("many underliners are given");
        }
    }
}
