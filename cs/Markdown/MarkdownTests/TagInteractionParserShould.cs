using System.Collections.Generic;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class TagInteractionParserShould
    {
        [TestCaseSource(nameof(CommonRulesExamples))]
        public void RecognizeCommonRulesLike(string input, List<TagEvent> expectedTags)
        {
            var tagEvents = ParseInput(input);

            tagEvents.Should().BeEquivalentTo(expectedTags);
        }

        private static List<TagEvent> ParseInput(string input)
        {
            var tags = new Taginizer(input).Taginize();
            new EscapingTagParser(tags).Parse();
            new UnderlineTagParser(tags, TagName.Underliner).Parse();
            new UnderlineTagParser(tags, TagName.DoubleUnderliner).Parse();
            new UnderlineParserCorrector(tags).Parse();
            new TagInteractionParser(tags).Parse();
            return tags;
        }

        private static IEnumerable<TestCaseData> CommonRulesExamples()
        {
            yield return new TestCaseData(
                "__hello _single_ underlining!__",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "hello"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "single"),
                    new TagEvent(TagSide.Right, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "underlining!"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Eof, ""),
                }).SetName("single underlining may be inside double underlining");
            yield return new TestCaseData(
                "# simple header\n",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.None, TagName.Word, "simple"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "header"),
                    new TagEvent(TagSide.Right, TagName.NewLine, "\n"),
                    new TagEvent(TagSide.None, TagName.Eof, ""),
                }).SetName("simple header with new line is given");
            yield return new TestCaseData(
                "# _simple_ __header__",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.Left, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "simple"),
                    new TagEvent(TagSide.Right, TagName.Underliner, "_"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.Left, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "header"),
                    new TagEvent(TagSide.Right, TagName.DoubleUnderliner, "__"),
                    new TagEvent(TagSide.Right, TagName.Header, ""),
                }).SetName("simple underlining inside header");
            yield return new TestCaseData(
                "# _simple __header_",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.None, TagName.Word, "_"),
                    new TagEvent(TagSide.None, TagName.Word, "simple"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "header"),
                    new TagEvent(TagSide.None, TagName.Word, "_"),
                    new TagEvent(TagSide.Right, TagName.Header, ""),
                }).SetName("intersected underlines are words");
            yield return new TestCaseData(
                "# simple __header",
                new List<TagEvent>
                {
                    new TagEvent(TagSide.Left, TagName.Header, "# "),
                    new TagEvent(TagSide.None, TagName.Word, "simple"),
                    new TagEvent(TagSide.None, TagName.Whitespace, " "),
                    new TagEvent(TagSide.None, TagName.Word, "__"),
                    new TagEvent(TagSide.None, TagName.Word, "header"),
                    new TagEvent(TagSide.Right, TagName.Header, ""),
                }).SetName("unpaired underliner inside header is word");

        }
    }
}
