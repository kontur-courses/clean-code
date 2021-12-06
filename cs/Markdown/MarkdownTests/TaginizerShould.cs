using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using Markdown.TagEvents;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class TaginizerShould
    {
        [TestCaseSource(nameof(SimpleCases))]
        public void TaginizeSimpleStringCorrectly(string input, List<TagEvent> expectedTagEvents)
        {
            var taginizer = new Taginizer(input);

            var tagEvents = taginizer.Taginize();

            tagEvents.Should().BeEquivalentTo(expectedTagEvents);
        }

        public static IEnumerable<TestCaseData> SimpleCases()
        {
            yield return new TestCaseData(
                "####hi\\peo5ple___ h  _\n\\##_ ___",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "#"),
                    new TagEvent(TagSide.None, TagName.Word, "###"),
                    new TagEvent(TagSide.None, TagName.Word, "hi"),
                    new TagEvent(TagSide.None, TagName.Escape, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, "peo"),
                    new TagEvent(TagSide.None, TagName.Number, "5"),
                    new TagEvent(TagSide.None, TagName.Word, "ple"),
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "h"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.Right, TagName.NewLine, "\n"),
                    new TagEvent(TagSide.None, TagName.Escape, "\\"),
                    new TagEvent(TagSide.None, TagName.Word, "##"),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("several different symbols");
        }
    }
}
