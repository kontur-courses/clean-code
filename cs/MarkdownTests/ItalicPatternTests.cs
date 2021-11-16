using System.Collections.Generic;
using FluentAssertions;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class ItalicPatternTest
    {
        [TestCaseSource(nameof(IsStartShouldBeTrueCases))]
        public void IsStart_ShouldBeTrue(Context context) => new ItalicPattern().IsStart(context).Should().BeTrue();

        public static IEnumerable<TestCaseData> IsStartShouldBeTrueCases()
        {
            yield return new TestCaseData(new Context("_a")) {TestName = "Before letter"};
            yield return new TestCaseData(new Context("a_b", 1)) {TestName = "In the middle of a word"};
        }

        [TestCaseSource(nameof(IsStartShouldBeFalseCases))]
        public void IsStart_ShouldBeFalse(Context context) => new ItalicPattern().IsStart(context).Should().BeFalse();

        public static IEnumerable<TestCaseData> IsStartShouldBeFalseCases()
        {
            yield return new TestCaseData(new Context("U")) {TestName = "When symbol is wrong"};
            yield return new TestCaseData(new Context("_")) {TestName = "When symbol is alone"};
            yield return new TestCaseData(new Context("_1")) {TestName = "When next symbol is digit"};
            yield return new TestCaseData(new Context("_ ")) {TestName = "When next symbol is space"};
            yield return new TestCaseData(new Context("__")) {TestName = "When next symbol underline"};
        }

        [TestCaseSource(nameof(IsEndShouldBeTrueCases))]
        public void IsEnd_ShouldBeTrue(Context context) => new ItalicPattern().IsEnd(context).Should().BeTrue();

        public static IEnumerable<TestCaseData> IsEndShouldBeTrueCases()
        {
            yield return new TestCaseData(new Context("a_", 1)) {TestName = "After letter"};
            yield return new TestCaseData(new Context("a_b", 1)) {TestName = "In the middle of word"};
        }

        [TestCaseSource(nameof(IsEndShouldBeFalseCases))]
        public void IsEnd_ShouldBeFalse(Context context) => new ItalicPattern().IsEnd(context).Should().BeFalse();

        public static IEnumerable<TestCaseData> IsEndShouldBeFalseCases()
        {
            yield return new TestCaseData(new Context("U")) {TestName = "When symbol is wrong"};
            yield return new TestCaseData(new Context("_")) {TestName = "When symbol is alone"};
            yield return new TestCaseData(new Context("1_", 1)) {TestName = "When previous symbol is digit"};
            yield return new TestCaseData(new Context(" _", 1)) {TestName = "When previous symbol is space"};
            yield return new TestCaseData(new Context("__", 1)) {TestName = "When previous symbol is underline"};
        }
    }
}