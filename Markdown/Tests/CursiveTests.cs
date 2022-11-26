using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class CursiveTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Render_ShouldRenderCursive()
        {
            Markdown.Markdown.Render("Text _with_ text")
                .Should().Be(@"Text <em>with<\em> text");
        }

        [Test]
        public void Render_ShouldNotRenderCursive_WhenInconsistent()
        {
            Markdown.Markdown.Render("Text _with_ text _ab")
                .Should().Be(@"Text <em>with<\em> text _ab");
        }

        [Test]
        public void Render_ShouldRenderBold_WhenAllLineIsCursive()
        {
            Markdown.Markdown.Render("_abbbac_")
                .Should().Be(@"<em>abbbac<\em>");
        }

        [Test]
        public void Render_ShouldBotRenderCursive_WhenContainsDigits()
        {
            Markdown.Markdown.Render("Text _abc 1 abc_ text")
                .Should().Be(@"Text _abc 1 abc_ text");
        }


        [Test]
        public void Render_ShouldNotRenderCursive_WhenContainsPartsOfDifferentWords()
        {
            Markdown.Markdown.Render("Tex_t te_xt")
                .Should().Be(@"Tex_t te_xt");
        }

        [Test]
        public void Render_ShouldRenderCursive_WhenContainsOnlyBeginningOfOneWord()
        {
            Markdown.Markdown.Render("abc _te_xt")
                .Should().Be(@"abc <em>te<\em>xt");
            Markdown.Markdown.Render("_te_xt")
                .Should().Be(@"<em>te<\em>xt");
        }

        [Test]
        public void Render_ShouldRenderCursive_WhenContainsOnlyEndingOfOneWord()
        {
            Markdown.Markdown.Render("te_xt_")
                .Should().Be(@"te<em>xt<\em>");
            Markdown.Markdown.Render("te_xt_ abc")
                .Should().Be(@"te<em>xt<\em> abc");
        }

        [Test]
        public void Render_ShouldRenderCursive_WhenContainsOnlyMiddlePartOfOneWord()
        {
            Markdown.Markdown.Render("t_ex_t")
                .Should().Be(@"t<em>ex<\em>t");
        }
    }
}