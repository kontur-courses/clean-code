using System.Collections.Generic;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class UnderlinerParserCorrectorShould
    {
        [TestCaseSource(nameof(CasesToCorrect))]
        public void CorrectLeftUnderlinersWhen(string input,
            List<TagEvent> tagEvents)
        {
            var tags = new Taginizer(input).Taginize();
            new EscapingTagParser(tags).Parse();
            new UnderlineTagParser(tags, TagName.Underliner).Parse();
            new UnderlineTagParser(tags, TagName.DoubleUnderliner).Parse();
            new UnderlineParserCorrector(tags).Parse();

            tags.Should().BeEquivalentTo(tagEvents);
        }

        private static IEnumerable<TestCaseData> CasesToCorrect()
        {
            yield return new TestCaseData(
                "__hello _my friend__",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "my"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "friend"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("closing tag at the end of the sentence");
            yield return new TestCaseData(
                "_ __ __",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.None, TagName.Word, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "__"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("separated underliners are given");
        }
    }
}
