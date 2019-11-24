using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCaseSource(nameof(EscapeCharacterTestCases))]
        public void RenderHtml_OnDifferentCasesWithEscapeCharacters(string markdownText, string expectedHtml)
        {
            var actualResult = Markdown.RenderHtml(markdownText);

            actualResult.Should().Be(expectedHtml);
        }

        private static IEnumerable<TestCaseData> EscapeCharacterTestCases
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                yield return new TestCaseData(
                        @"they have \_twelve scraps\_ of paper... twelve chances to kill!",
                        "they have _twelve scraps_ of paper... twelve chances to kill!")
                    .SetName("WhenEscapeCharacterBeforeUnderscore_DoesntReplace");
                yield return new TestCaseData(
                        @"they have twelve \\scraps of paper... twelve chances to kill!",
                        @"they have twelve \scraps of paper... twelve chances to kill!")
                    .SetName("WhenDoubleEscapeCharacters_OneOfThemDisappear");
                yield return new TestCaseData(
                        @"they have twelve \\\\\\scraps of paper... twelve chances to kill!",
                        @"they have twelve \\\scraps of paper... twelve chances to kill!")
                    .SetName("WhenSeveralEscapeCharacters_HalfOfThemDisappear");
            }
        }

        [TestCaseSource(nameof(UnderscoreTestCases))]
        public void RenderHtml_OnDifferentCasesWithUnderscores(string markdownText, string expectedHtml)
        {
            var actualResult = Markdown.RenderHtml(markdownText);

            actualResult.Should().Be(expectedHtml);
        }

        private static IEnumerable<TestCaseData> UnderscoreTestCases
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                yield return new TestCaseData(
                        "they have _twelve scraps_ of paper... twelve chances to kill!",
                        "they have <em>twelve scraps</em> of paper... twelve chances to kill!")
                    .SetName("WhenCorrectlySingleUnderscoresWrap_ReplacesUnderscoresWithEmTag");
                yield return new TestCaseData(
                        "they have __twelve scraps__ of paper... twelve chances to kill!",
                        "they have <strong>twelve scraps</strong> of paper... twelve chances to kill!")
                    .SetName("WhenCorrectlyDoubleUnderscoresWrap_ReplacesUnderscoresWithStrongTag");
                yield return new TestCaseData(
                        "they have __twelve _scraps of_ paper...__ twelve chance..",
                        "they have <strong>twelve <em>scraps of</em> paper...</strong> twelve chance..")
                    .SetName("WhenSingleUnderscoreWrapInsideOfDoubleUnderscoreWrap_ReplacesUnderscoresForBothWraps");
                yield return new TestCaseData(
                        "they have _twelve __scraps of__ paper..._ twelve chances to kill!",
                        "they have <em>twelve __scraps of__ paper...</em> twelve chances to kill!")
                    .SetName("WhenDoubleUnderscoresWrapInsideOfSingleUnderscoreWrap_ReplacesJustOuterWrap");
            }
        }

        [TestCaseSource(nameof(UnderscoreTestCasesWithSameInputAndResult))]
        public string RenderHtml_OnDifferentCasesWithUnderscores_ReturnsInputWithoutChanges(string markdownText) =>
            Markdown.RenderHtml(markdownText);

        private static IEnumerable<TestCaseData> UnderscoreTestCasesWithSameInputAndResult
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                {
                    const string markdownText = "they have __twelve scraps_ of paper... twelve chances to kill!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenNotPairWrapMarkers_ReplacesUnderscoresWithEmTag");
                }
                {
                    const string markdownText = @"they have twelve sc_rap_s of paper... twelve chances to kill!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenUnderscoresWithoutSpacesAround_DoesntReplace");
                }
                {
                    const string markdownText = @"they have_ twelve scraps_ of paper... twelve chances to kill!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenSpaceAfterOpenWrapUnderscore_DoesntReplace");
                }
                {
                    const string markdownText = @"they have _twelve scraps _of paper... twelve chances to kill!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenSpaceBeforeCloseWrapUnderscore_DoesntReplace");
                }
            }
        }
    }
}