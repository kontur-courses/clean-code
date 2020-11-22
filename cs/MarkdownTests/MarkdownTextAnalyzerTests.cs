using System.Collections;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MarkdownTextAnalyzerTests
    {
        public static IEnumerable TestCasesForGetAllTags
        {
            get
            {
                yield return new TestCaseData("#a", new[] {"<h1>", "</h1>"}, new[] {0, 2}).SetName("WhenOneTitle");
                yield return new TestCaseData("\\#a", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenEscapedMarkdown");
                yield return new TestCaseData("_a_", new[] {"<em>", "</em>"}, new[] {0, 2}).SetName(
                    "WhenThereIsOneItalicField");
                yield return new TestCaseData("_a_ _b_", new[] {"<em>", "</em>", "<em>", "</em>"}, new[] {0, 2, 4, 6})
                    .SetName(
                        "WhenThereAreSeveralItalicField");
                yield return new TestCaseData("_a", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenThereIsNoClosingMarkdown");
                yield return new TestCaseData("_ab_c", new[] {"<em>", "</em>"}, new[] {0, 3}).SetName(
                    "WhenMarkdownsSelectPartOfWord");
                yield return new TestCaseData("a_b c_d", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenMarkdownsInDifferentWords");
                yield return new TestCaseData("_ ac_", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenWhitespaceAfterOpeningMarkdown");
                yield return new TestCaseData("_ac _", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenWhitespaceBeforeClosingMarkdown");
                yield return new TestCaseData("\\_a_", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenEscapedMarkdown");
                yield return new TestCaseData("_12_3", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenMarkdownInsideDigits");
                yield return new TestCaseData("__a__", new[] {"<strong>", "</strong>"}, new[] {0, 3}).SetName(
                    "WhenThereIsOneBoldField");
                yield return new TestCaseData("__a__ __b__", new[] {"<strong>", "</strong>", "<strong>", "</strong>"},
                    new[] {0, 3, 6, 9}).SetName("WhenThereAreSeveralBoldField");
                yield return new TestCaseData("__a", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenThereIsNoClosingMarkdown");
                yield return new TestCaseData("__ab__c", new[] {"<strong>", "</strong>"}, new[] {0, 4}).SetName(
                    "WhenMarkdownsSelectPartOfWord");
                yield return new TestCaseData("a__b c__d", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenMarkdownsInDifferentWords");
                yield return new TestCaseData("__ ac__", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenWhitespaceAfterOpeningMarkdown");
                yield return new TestCaseData("__ac __", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenWhitespaceBeforeClosingMarkdown");
                yield return new TestCaseData("____", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenEmptyLineInsideBold");
                yield return new TestCaseData("__12__3", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenMarkdownSelectDigits");
                yield return new TestCaseData("#__a__ _b_",
                    new[] {"<h1>", "</h1>", "<strong>", "</strong>", "<em>", "</em>"},
                    new[] {0, 10, 1, 4, 7, 9}).SetName(
                    "WhenThereAllCorrectMarkdowns");
                yield return new TestCaseData("__a_b__c_", new string[0], new int[0]).SetName(
                    "ReturnEmptyCollection_WhenIntersectionOfItalicAndBoldMarkdowns");
                yield return new TestCaseData("_a __b__ c_", new[] {"<em>", "</em>"}, new[] {0, 10}).SetName(
                    "ReturnOnlyItalicToken_WhenBoldInsideItalicMarkdowns");
                yield return new TestCaseData("__a _b_ c__", new[] {"<em>", "</em>", "<strong>", "</strong>"},
                    new[] {4, 6, 0, 9}).SetName(
                    "WhenMarkdownsSelectPartOfWord");
            }
        }

        [Test]
        [TestCaseSource("TestCasesForGetAllTags")]
        public void GetAllTagsTest(string text, string[] expectedTagsValues, int[] expectedPositions)
        {
            var actualTags = MarkdownTextAnalyzer.GetAllTags(text).ToArray();
            actualTags.Length.Should().Be(expectedTagsValues.Length);
            for (var i = 0; i < actualTags.Length; i++)
            {
                actualTags[i].Value.Should().Be(expectedTagsValues[i]);
                actualTags[i].Position.Should().Be(expectedPositions[i]);
            }
        }
    }
}