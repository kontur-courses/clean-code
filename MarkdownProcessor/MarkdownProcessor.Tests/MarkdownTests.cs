using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [Test]
        public void RenderHtml_OnCorrectSingleUnderscoresWrap_ReplacesUnderscoresWithEmTag()
        {
            const string markdownText = "they have _twelve scraps_ of paper... twelve chances to kill!";
            const string expectedHtml = "they have <em>twelve scraps</em> of paper... twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnCorrectDoubleUnderscoresWrap_ReplacesUnderscoresWithStrongTag()
        {
            const string markdownText = "they have __twelve scraps__ of paper... twelve chances to kill!";
            const string expectedHtml = "they have <strong>twelve scraps</strong> of paper... twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnSingleUnderscoreWrapInsideOfDoubleUnderscoreWrap_ReplacesUnderscoresForBothWraps()
        {
            const string markdownText = "they have __twelve _scraps of_ paper...__ twelve chance..";
            const string expectedHtml = "they have <strong>twelve <em>scraps of</em> paper...</strong> twelve chance..";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnDoubleUnderscoreWrapInsideOfSingleUnderscoreWrap_ReplacesUnderscoresJustForOuterWrap()
        {
            const string markdownText = "they have _twelve __scraps of__ paper..._ twelve chances to kill!";
            const string expectedHtml = "they have <em>twelve __scraps of__ paper...</em> twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnNotPairWrapMarkers_ReplacesUnderscoresWithEmTag()
        {
            const string markdownText = "they have __twelve scraps_ of paper... twelve chances to kill!";
            const string expectedHtml = markdownText;

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_EscapeCharacterBeforeUnderscore_DoesntReplace()
        {
            const string markdownText = @"they have \_twelve scraps\_ of paper... twelve chances to kill!";
            const string expectedHtml = "they have _twelve scraps_ of paper... twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_DoubleEscapeCharacters_OneOfThemDisappear()
        {
            const string markdownText = @"they have twelve \\scraps of paper... twelve chances to kill!";
            const string expectedHtml = @"they have twelve \scraps of paper... twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnEvenCountOfEscapeCharacters_HalfOfThemDisappear()
        {
            const string markdownText = @"they have twelve \\\\\\scraps of paper... twelve chances to kill!";
            const string expectedHtml = @"they have twelve \\\scraps of paper... twelve chances to kill!";

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnUnderscoresWithoutSpacesAround_DoesntReplace()
        {
            const string markdownText = @"they have twelve sc_rap_s of paper... twelve chances to kill!";
            const string expectedHtml = markdownText;

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnSpaceAfterOpenWrapUnderscore_DoesntReplace()
        {
            const string markdownText = @"they have_ twelve scraps_ of paper... twelve chances to kill!";
            const string expectedHtml = markdownText;

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }

        [Test]
        public void RenderHtml_OnSpaceBeforeCloseWrapUnderscore_DoesntReplace()
        {
            const string markdownText = @"they have _twelve scraps _of paper... twelve chances to kill!";
            const string expectedHtml = markdownText;

            var actualHtml = Markdown.RenderHtml(markdownText);

            actualHtml.Should().Be(expectedHtml);
        }
    }
}