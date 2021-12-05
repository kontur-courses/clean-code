using System.Collections.Generic;
using NUnit;
using FluentAssertions;
using Markdown.Tag_Classes;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class TagFiniteStateMachineShould
    {
        [TestCaseSource(nameof(SimpleSingleUnderlinerTagCases))]
        [TestCaseSource(nameof(SimpleDoubleUnderlinerTagCases))]
        [TestCaseSource(nameof(UnderlineDigitsIntercation))]
        [TestCaseSource(nameof(EscapeCases))]
        public void ParseSimpleTagCasesCorrectly(string input, List<TagEvent> expectedTagEvents)
        {
            var stateMachine = new TagFiniteStateMachine();

            var tagEvents = stateMachine.GetTagEvents(input);

            tagEvents.Should().BeEquivalentTo(expectedTagEvents);
        }

        public static IEnumerable<TestCaseData> EscapeCases()
        {
            yield return new TestCaseData(
                "\\ \\",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                }).SetName("whitespace between slashes");
            yield return new TestCaseData(
                "\\",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                }).SetName("single slash");
            yield return new TestCaseData(
                "hel\\lo",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "hel"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Text, "lo"),
                }).SetName("escape text");
            yield return new TestCaseData(
                "hel\\\\lo",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "hel"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Text, "lo"),
                }).SetName("escape slash");
            yield return new TestCaseData(
                "hel\\\\\\lo",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "hel"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Text, "lo"),
                }).SetName("three slashes in a row inside word");
            yield return new TestCaseData(
                "hel\\ lo",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "hel"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.None, Mark.Text, " lo"),
                }).SetName("escape whitespace");
            yield return new TestCaseData(
                "\\_hello\\_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "hello"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                }).SetName("escape single underliners");
            yield return new TestCaseData(
                "\\__hello\\__",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "hello"),
                    new TagEvent(Side.None, Mark.Escape, "\\"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("escape double underliners");
        }

        public static IEnumerable<TestCaseData> UnderlineEscaping()
        {
            yield return new TestCaseData(
                "\\_hello\\_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "12_3"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("shield simple underliners");
            yield return new TestCaseData(
                "_123_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "123"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("underline number");
        }

        public static IEnumerable<TestCaseData> UnderlineDigitsIntercation()
        {
            yield return new TestCaseData(
                "_12_3_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "12_3"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("not underline between digits");
            yield return new TestCaseData(
                "_123_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "123"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("underline number");
        }

        public static IEnumerable<TestCaseData> SimpleSingleUnderlinerTagCases()
        {
            yield return new TestCaseData(
                " _ _ _word_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "word"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("left underlines without pair when they underlines nothing");
            yield return new TestCaseData(
                "fir_st wo_rd",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st wo"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "rd")
                }).SetName("single underliners in different words");
            yield return new TestCaseData(
                "_first_",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "first"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("word single underlining");
            yield return new TestCaseData(
                "first_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "first"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                }).SetName("one single inderline in the word end");
            yield return new TestCaseData(
                "first word",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "first word"),
                }).SetName("two words without underliners");
            yield return new TestCaseData(
                "_fir_st",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                }).SetName("word beginning with single underlining");
            yield return new TestCaseData(
                "fi_r_st",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fi"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "r"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                }).SetName("word middle with single underlining");
            yield return new TestCaseData(
                "fir_st_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("word ending with single underlining");
            yield return new TestCaseData(
                "hi_ _hello",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "hi"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "hello"),
                }).SetName("word ending underlining not make pair with next underlining");
        }

        public static IEnumerable<TestCaseData> SimpleDoubleUnderlinerTagCases()
        {
            yield return new TestCaseData(
                " __ __ __word__",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "word"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("left double underlines without pair when they underlining nothing");
            yield return new TestCaseData(
                "fir__st wo__rd",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st wo"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "rd")
                }).SetName("double underliners in different words");
            yield return new TestCaseData(
                "__first__",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "first"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("double underline whole word");
            yield return new TestCaseData(
                "first__",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "first"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("only closing double underliner is given");
            yield return new TestCaseData(
                "__fir__st",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                }).SetName("word beginning is double underlied");
            yield return new TestCaseData(
                "fi__r__st",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fi"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "r"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                }).SetName("word middle is double underlied");
            yield return new TestCaseData(
                "fir__st__",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("right part of the word is double underlied");
            yield return new TestCaseData(
                "st__ __hello",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "st"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "hello"),
                }).SetName("word ending double underlining not make pair with next double underlining");
        }
    }
}
