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
                yield return new TestCaseData(@"\_q w\_",
                                              @"_q w_")
                    .SetName("WhenEscapeCharacterBeforeUnderscore_DoesntReplace");
                yield return new TestCaseData(@"\\qwerty...",
                                              @"\qwerty...")
                    .SetName("WhenDoubleEscapeCharacters_OneOfThemDisappear");
                yield return new TestCaseData(@"\\\\\\qwerty",
                                              @"\\\qwerty")
                    .SetName("WhenSeveralEscapeCharacters_HalfOfThemDisappear");
                yield return new TestCaseData(@"\_qwe_",
                                              @"_qwe_")
                    .SetName("WhenOnlyFirstUnderscoreEscaped_DoesntReplace");
                yield return new TestCaseData(@"_qwe\_",
                                              @"_qwe_")
                    .SetName("WhenOnlySecondUnderscoreEscaped_DoesntReplace");
                yield return new TestCaseData(@"\\_qwe_",
                                              @"\<em>qwe</em>")
                    .SetName("WhenDoubleEscapedBeforeFirstUnderscore_ReplacedWithTagCorrectly");
                yield return new TestCaseData(@"_qwe\\_",
                                              @"<em>qwe\</em>")
                    .SetName("WhenDoubleEscapedBeforeSecondUnderscore_ReplacedWithTagCorrectly");
                yield return new TestCaseData(@"\__qwe__",
                                              @"__qwe__")
                    .SetName("WhenEscapedBeforeFirstDoubleUnderscore_DoesntReplace");
                yield return new TestCaseData(@"__qwe\__",
                                              @"__qwe__")
                    .SetName("WhenEscapedBeforeSecondDoubleUnderscore_DoesntReplace");
                yield return new TestCaseData(@"\\\\",
                                              @"\\")
                    .SetName("WhenOnlyEscapedCharactersEvenCount");
                yield return new TestCaseData(@"\\\",
                                              @"\\")
                    .SetName("WhenOnlyEscapedCharactersOddCount");
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
                yield return new TestCaseData("_qwe_",
                                              "<em>qwe</em>")
                    .SetName("WhenCorrectlySingleUnderscoresWrap_ReplacesUnderscoresWithEmTag");
                yield return new TestCaseData("q_w_e",
                                              "q<em>w</em>e")
                    .SetName("WhenCorrectlySingleUnderscoresWrapSurroundedBySymbols_ReplacesUnderscoresWithEmTag");
                yield return new TestCaseData("__qwe rty__",
                                              "<strong>qwe rty</strong>")
                    .SetName("WhenCorrectlyDoubleUnderscoresWrap_ReplacesUnderscoresWithStrongTag");
                yield return new TestCaseData("w__qwe rty__t",
                                              "w<strong>qwe rty</strong>t")
                    .SetName("WhenCorrectlyDoubleUnderscoresWrapSurroundBySymbols_ReplacesUnderscoresWithStrongTag");
                yield return new TestCaseData("__qwe _rty qwe_ rty__",
                                              "<strong>qwe <em>rty qwe</em> rty</strong>")
                    .SetName("WhenSingleUnderscoreWrapInsideOfDoubleUnderscoreWrap_ReplacesUnderscoresForBothWraps");
                yield return new TestCaseData("_qwe __rty__ tyu_",
                                              "<em>qwe __rty__ tyu</em>")
                    .SetName("WhenDoubleUnderscoresWrapInsideOfSingleUnderscoreWrap_ReplacesJustOuterWrap");
                yield return new TestCaseData("__qw_er__",
                                              "<strong>qw_er</strong>")
                    .SetName("WhenDoubleUnderscoresWrapContainsSingleUnderscore_ReplacesJustOuterWrap");
                yield return new TestCaseData("__qw_er___",
                                              "<strong>qw_er</strong>_")
                    .SetName("WhenWrapsIntersecting_ReplacesJustOuterDoubleWrap");
                yield return new TestCaseData("___qwe__rt_",
                                              "<strong>_qwe</strong>rt_")
                    .SetName("WhenUnderscoresInDescendingOrder");
                yield return new TestCaseData("_qwe__rt___",
                                              "<em>qwe__rt__</em>")
                    .SetName("WhenUnderscoresOrdered");
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
                    const string markdownText = "__q w_";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenNotPairWrapMarkers");
                }
                {
                    const string markdownText = @"hello, s1_2a9_5!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenUnderscoresWithDigitsAround");
                }
                {
                    const string markdownText = @"hello, _2a9_5!";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenUnderscoresAfterWhiteSpaceWithDigitsAround");
                }
                {
                    const string markdownText = @"hello, 3__2a9__";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenUnderscoresBeforeEndLineWithDigitsAround");
                }
                {
                    const string markdownText = @"q_ w e_";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenSpaceAfterOpenWrapUnderscore");
                }
                {
                    const string markdownText = @"q _w e _r";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenSpaceBeforeCloseWrapUnderscore");
                }
                {
                    const string markdownText = "_q";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenAloneUnderscoreBefore");
                }
                {
                    const string markdownText = "___q";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenTripleUnderscoreBefore");
                }
                {
                    const string markdownText = "q_";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenAloneUnderscoreAfter");
                }
                {
                    const string markdownText = "q___";
                    const string expectedHtml = markdownText;
                    yield return new TestCaseData(markdownText).Returns(expectedHtml).SetName(
                        "WhenTripleUnderscoreAfter");
                }
            }
        }
    }
}