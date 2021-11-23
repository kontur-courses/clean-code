﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Tokens.Patterns;
using NUnit.Framework;

namespace MarkdownTests
{
    public class PairedTokenPatternTests
    {
        private static readonly List<string> tagsToTest = new() {"_", "__"};

        [TestCaseSource(nameof(TrySetStartTrueCases))]
        public void TrySetStart_True(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .TrySetStart(context)
                .Should().BeTrue();

        [TestCaseSource(nameof(TrySetStartFalseCases))]
        public void TrySetStart_False(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .TrySetStart(context)
                .Should().BeFalse();

        [TestCaseSource(nameof(TryContinueFalseCases))]
        public void TryContinue_False(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .TryContinue(context)
                .Should().BeFalse();

        [TestCaseSource(nameof(tagsToTest))]
        public void TryContinue_False_TagInDifferentWord(string tag)
        {
            var context = new Context($"o{tag}ne to{tag}w");
            var pattern = new PairedTokenPattern(tag);

            context.Index = 1;
            pattern.TrySetStart(context);
            context.Index += tag.Length + 2;

            pattern.TryContinue(context)
                .Should().BeFalse();
        }

        [TestCaseSource(nameof(TryContinueTrueCases))]
        public void TryContinue_True(Context context, string tag) =>
            new PairedTokenPattern(tag)
                .TryContinue(context)
                .Should().BeTrue();

        [TestCaseSource(nameof(tagsToTest))]
        public void LastCloseSucceed_True_AfterTagClose(string tag)
        {
            var pattern = new PairedTokenPattern(tag);
            var context = new Context($"{tag}a{tag}");

            pattern.TrySetStart(context);
            context.Index = tag.Length + 1;
            pattern.TryContinue(context);

            pattern.LastEndingSucceed.Should().BeTrue();
        }

        [TestCaseSource(nameof(tagsToTest))]
        public void LastCloseSucceed_False_TagInDifferentWords(string tag)
        {
            var context = new Context($"o{tag}ne to{tag}w");
            var pattern = new PairedTokenPattern(tag);

            context.Index = 1;
            pattern.TrySetStart(context);
            context.Index += tag.Length + 2;
            pattern.TryContinue(context);

            pattern.LastEndingSucceed.Should().BeFalse();
        }

        private static IEnumerable<TestCaseData> TrySetStartTrueCases() =>
            GenerateCases(TrySetStartTrueCasesPattern);

        private static IEnumerable<TestCaseData> TrySetStartTrueCasesPattern(string tag)
        {
            yield return new TestCaseData(new Context($"{tag}a"), tag) {TestName = "Before letter"};
            yield return new TestCaseData(new Context($" {tag}a", 1), tag) {TestName = "Before letter after space"};
            yield return new TestCaseData(new Context($"a{tag}b", 1), tag) {TestName = "In the middle of a word"};
        }

        private static IEnumerable<TestCaseData> TrySetStartFalseCases =>
            GenerateCases(TrySetStartFalseCasesPattern);

        private static IEnumerable<TestCaseData> TrySetStartFalseCasesPattern(string tag)
        {
            yield return new TestCaseData(new Context("U"), tag) {TestName = "Symbol is wrong"};
            yield return new TestCaseData(new Context(tag), tag) {TestName = "Tag is alone"};
            yield return new TestCaseData(new Context($"a{tag}1", 1), tag) {TestName = "Next symbol is digit"};
            yield return new TestCaseData(new Context($"a{tag} ", 1), tag) {TestName = "Next symbol is space"};
            yield return new TestCaseData(new Context($"a{tag}_", 1), tag) {TestName = "Next symbol is underline"};
            yield return new TestCaseData(new Context($"_{tag}a", 1), tag) {TestName = "Previous symbol is underline"};
            yield return new TestCaseData(new Context($"1{tag}a", 1), tag) {TestName = "Previous symbol is digit"};
        }

        private static IEnumerable<TestCaseData> TryContinueFalseCases =>
            GenerateCases(TryContinueFalseCasesPattern);

        private static IEnumerable<TestCaseData> TryContinueFalseCasesPattern(string tag)
        {
            yield return new TestCaseData(new Context($"a{tag} ", 1), tag) {TestName = "After letter"};
            yield return new TestCaseData(new Context($"a{tag}b", 1), tag) {TestName = "In the middle of word"};
            yield return new TestCaseData(new Context($"a{tag}", 1), tag) {TestName = "At the end of a string"};
        }

        private static IEnumerable<TestCaseData> TryContinueTrueCases =>
            GenerateCases(TryContinueTrueCasesPattern);

        private static IEnumerable<TestCaseData> TryContinueTrueCasesPattern(string tag)
        {
            yield return new TestCaseData(new Context("U"), tag) {TestName = "Symbol is wrong"};
            yield return new TestCaseData(new Context(tag), tag) {TestName = "Tag is alone"};
            yield return new TestCaseData(new Context($"1{tag}", 1), tag) {TestName = "Previous symbol is digit"};
            yield return new TestCaseData(new Context($" {tag}", 1), tag) {TestName = "Previous symbol is space"};
            yield return new TestCaseData(new Context($"_{tag}", 1), tag) {TestName = "Previous symbol is underline"};
            yield return new TestCaseData(new Context($"a{tag}_", 1), tag) {TestName = "Next symbol is underline"};
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