using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [SetUp]
        public void SetUp()
        {
            sut = new Markdown();
        }

        public Markdown sut;

        [Test]
        public void Render_OrdinaryText_ExactText()
        {
            var mdText = "hello";
            var htmlText = sut.Render(mdText);
            htmlText.Should().Be(mdText);
        }

        [Test]
        public void Render_TextWithSpecialSymbols_CorrectHTMLText()
        {
            var mdText = "__la _la `la` la_ la__";
            var expectedHtmlText = "<strong>la <em>la <code>la</code> la</em> la</strong>";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_EscapedSymbols_NotShowBackslash()
        {
            var mdText = "\\__la";
            var expectedHtmlText = "__la";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_OnePairAndOneEscapeDSymbol()
        {
            var mdText = "__la \\`lalala__";
            var expectedHtmlText = "<strong>la `lalala</strong>";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void Render_WrongWhiteSpacePosition_ExactText()
        {
            var mdText = "__ la lalala __";
            var actualHtmlText = sut.Render(mdText);
            actualHtmlText.Should().Be(mdText);
        }
    }
}