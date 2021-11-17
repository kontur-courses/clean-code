using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class PairedTokenPatternTests
    {
        private static readonly List<string> tagsToTest = new() {"_", "__"};

        [TestCaseSource(nameof(IsStartShouldBeTrueWithOneSymbolTagCases))]
        public void IsStart_ShouldBeTrue_WithOneSymbolTag(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .IsStart(context)
                .Should().BeTrue();

        public static IEnumerable<TestCaseData> IsStartShouldBeTrueWithOneSymbolTagCases() =>
            GenerateCases(GenerateIsStartShouldBeTrueCases);


        [TestCaseSource(nameof(IsStartShouldBeFalseWithOneSymbolTagCases))]
        public void IsStart_ShouldBeFalse_WithOneSymbolTag(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .IsStart(context)
                .Should().BeFalse();

        public static IEnumerable<TestCaseData> IsStartShouldBeFalseWithOneSymbolTagCases =>
            GenerateCases(GenerateIsStartShouldBeFalseCases);

        [TestCaseSource(nameof(GenerateIsEndShouldBeTrueWithOneSymbolTagCases))]
        public void IsEnd_ShouldBeTrue_WithOneSymbolTag(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .IsEnd(context)
                .Should().BeTrue();

        public static IEnumerable<TestCaseData> GenerateIsEndShouldBeTrueWithOneSymbolTagCases =>
            GenerateCases(GenerateIsEndShouldBeTrueCases);

        [TestCaseSource(nameof(IsEndShouldBeFalseCasesWithOneSymbolTag))]
        public void IsEnd_ShouldBeFalse_WithOneSymbolTag(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .IsEnd(context)
                .Should().BeFalse();

        public static IEnumerable<TestCaseData> IsEndShouldBeFalseCasesWithOneSymbolTag =>
            GenerateCases(GenerateIsEndShouldBeFalseCases);

        private static IEnumerable<TestCaseData> GenerateIsStartShouldBeTrueCases(string tag)
        {
            yield return new TestCaseData(new Context($"{tag}a"), tag) {TestName = "Before letter"};
            yield return new TestCaseData(new Context($"a{tag}b", 1), tag) {TestName = "In the middle of a word"};
        }

        private static IEnumerable<TestCaseData> GenerateIsStartShouldBeFalseCases(string tag)
        {
            yield return new TestCaseData(new Context("U"), tag) {TestName = "When symbol is wrong"};
            yield return new TestCaseData(new Context(tag), tag) {TestName = "When symbol is alone"};
            yield return new TestCaseData(new Context($"{tag}1"), tag) {TestName = "When next symbol is digit"};
            yield return new TestCaseData(new Context($"{tag} "), tag) {TestName = "When next symbol is space"};
            yield return new TestCaseData(new Context($"{tag}_"), tag) {TestName = "When next symbol underline"};
        }

        private static IEnumerable<TestCaseData> GenerateIsEndShouldBeTrueCases(string tag)
        {
            yield return new TestCaseData(new Context($"a{tag}", 1), tag) {TestName = "After letter"};
            yield return new TestCaseData(new Context($"a{tag}b", 1), tag) {TestName = "In the middle of word"};
        }

        private static IEnumerable<TestCaseData> GenerateIsEndShouldBeFalseCases(string tag)
        {
            yield return new TestCaseData(new Context("U"), tag) {TestName = "When symbol is wrong"};
            yield return new TestCaseData(new Context(tag), tag) {TestName = "When symbol is alone"};
            yield return new TestCaseData(new Context($"1{tag}", 1), tag) {TestName = "When previous symbol is digit"};
            yield return new TestCaseData(new Context($" {tag}", 1), tag) {TestName = "When previous symbol is space"};
            yield return new TestCaseData(new Context($"_{tag}", 1), tag)
                {TestName = "When previous symbol is underline"};
        }

        private static IEnumerable<TestCaseData> GenerateCases(Func<string, IEnumerable<TestCaseData>> caseGenerator)
        {
            var testCases = new List<TestCaseData>();
            foreach (var tag in tagsToTest)
            {
                var generatedCases = caseGenerator(tag).ToList();
                generatedCases.ForEach(testCase => testCase.TestName += $" and tag = {tag}");
                testCases.AddRange(generatedCases);
            }

            return testCases;
        }
    }
}