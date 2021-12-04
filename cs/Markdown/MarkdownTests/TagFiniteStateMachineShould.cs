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
        [TestCaseSource(nameof(UnderlineDigitsIntercation))]
        public void ParseSimpleTagCasesCorrectly(string input, List<TagEvent> expectedTagEvents)
        {
            var stateMachine = new TagFiniteStateMachine();

            var tagEvents = stateMachine.GetTagEvents(input);

            tagEvents.Should().BeEquivalentTo(expectedTagEvents);
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
                }).SetName("underline whole word");
            yield return new TestCaseData(
                "first_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "first"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                }).SetName("only closing underliner is given");
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
                }).SetName("left part of the word is underlied");
            yield return new TestCaseData(
                "fi_r_st",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fi"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "r"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                }).SetName("word middle is underlied");
            yield return new TestCaseData(
                "fir_st_",
                new List<TagEvent>
                {
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                }).SetName("right part of the word is underlied");
            yield return new TestCaseData(
                "_fir_st_ _hello",
                new List<TagEvent>
                {
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "fir"),
                    new TagEvent(Side.Right, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "st"),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, " "),
                    new TagEvent(Side.Left, Mark.Underliner, "_"),
                    new TagEvent(Side.None, Mark.Text, "hello"),
                }).SetName("put left tag after left tag and whitespace");
        }
    }
}
