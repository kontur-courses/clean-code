using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class BoldTests
    {
        [Test]
        public void Render_ShouldRender_BasicBold()
        {
            Markdown.Markdown.Render("Text __with__ text")
                .Should().Be(@"Text <strong>with<\strong> text");
        }

        [Test]
        public void Render_ShouldNotRenderBold_WhenInconsistent()
        {
            Markdown.Markdown.Render("Text __with__ text __ab")
                .Should().Be(@"Text <strong>with<\strong> text __ab");
        }

        [Test]
        public void Render_ShouldRenderBold_WhenAllLineIsBold()
        {
            Markdown.Markdown.Render("__abbbac__")
                .Should().Be(@"<strong>abbbac<\strong>");
        }

        [Test]
        public void Render_ShouldBotRenderBold_WhenContainsDigits()
        {
            Markdown.Markdown.Render("Text __abc 1 abc__ Text")
                .Should().Be(@"Text __abc 1 abc__ Text");
        }

        [Test]
        public void Render_ShouldNotRenderBold_WhenContainsPartsOfDifferentWords()
        {
            Markdown.Markdown.Render("Tex__t te__xt")
                .Should().Be(@"Tex__t te__xt");
        }

        [Test]
        public void Render_ShouldRenderBold_WhenContainsOnlyBeginningOfOneWord()
        {
            Markdown.Markdown.Render("abc __te__xt")
                .Should().Be(@"abc <strong>te<\strong>xt");
            Markdown.Markdown.Render("__te__xt")
                .Should().Be(@"<strong>te<\strong>xt");
        }

        [Test]
        public void Render_ShouldRenderBold_WhenContainsOnlyEndingOfOneWord()
        {
            Markdown.Markdown.Render("te__xt__")
                .Should().Be(@"te<strong>xt<\strong>");
            Markdown.Markdown.Render("te__xt__ abc")
                .Should().Be(@"te<strong>xt<\strong> abc");
        }

        [Test]
        public void Render_ShouldRenderBold_WhenContainsOnlyMiddlePartOfOneWord()
        {
            Markdown.Markdown.Render("t__ex__t")
                .Should().Be(@"t<strong>ex<\strong>t");
        }
    }
}