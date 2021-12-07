using System.Collections.Generic;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class UnderlineTagParserShould
    {
        [TestCaseSource(nameof(SimpleCases))]
        public void RecognizeUnderlineTagsCorrectlyWhen(string input, List<TagEvent> tagEvents, 
            TagName underlineKind)
        {
            var tags = new Taginizer(input).Taginize();
            new EscapingTagParser(tags).Parse();

            new UnderlineTagParser(tags, underlineKind).Parse();

            tags.Should().BeEquivalentTo(tagEvents);
        }

        [TestCaseSource(nameof(DifferentUnderliningsCases))]
        public void RecognizeDifferentUnderlinings(string input, List<TagEvent> tagEvents)
        {
            var tags = new Taginizer(input).Taginize();
            new EscapingTagParser(tags).Parse();

            new UnderlineTagParser(tags, TagName.Underliner).Parse();
            new UnderlineTagParser(tags, TagName.DoubleUnderliner).Parse();

            tags.Should().BeEquivalentTo(tagEvents);
        }

        private static IEnumerable<TestCaseData> DifferentUnderliningsCases()
        {
            yield return new TestCaseData(
                "__hello_",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("simple intersection is given");
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
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                }).SetName("simple intersection is given");
        }

        private static IEnumerable<TestCaseData> SimpleCases()
        {
            yield return new TestCaseData(
                "_hello_",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.Right, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("word with single underlining is given");
            yield return new TestCaseData(
                "_123_",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Number, "123"),
                    new TagEvent(TagSide.Right, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("number with single underlining is given");
            yield return new TestCaseData(
                "_12_3",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Number, "12"),
                    new TagEvent(TagSide.None, TagName.Word, "_"),
                    new TagEvent(TagSide.None, TagName.Number, "3"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("single underlining within number is given");
            yield return new TestCaseData(
                "hel_lo wo_rld!",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.None, TagName.Word, "hel"),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "lo"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "wo"),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "rld!"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("when underlinings lies in differnet words");
            yield return new TestCaseData(
                "_hel_lo",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "hel"),
                    new TagEvent(TagSide.Right, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "lo"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("left part of word is underlied");
            yield return new TestCaseData(
                "h__el__lo",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.None, TagName.Word, "h"),
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "el"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "lo"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.DoubleUnderliner).SetName("middle part of word is underlied");
            yield return new TestCaseData(
                "hel__lo__",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.None, TagName.Word, "hel"),
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "lo"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.DoubleUnderliner).SetName("right part of word is underlied");
            yield return new TestCaseData(
                "____",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.DoubleUnderliner).SetName("empty string between double underliners is given");
            yield return new TestCaseData(
                "_ hello _",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.Underliner).SetName("there are whitespaces between word and single underliners");
            yield return new TestCaseData(
                "__hello__",
                new List<TagEvent>()
                {
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, "")
                },
                TagName.DoubleUnderliner).SetName("word with double underlining");
        }
    }
}
