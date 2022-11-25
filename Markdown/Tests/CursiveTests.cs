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
        public void BasicCursive()
        {
            Markdown.Markdown.Render("Text _with_ text")
                .Should().Be(@"Text <em>with<\em> text");
        }

        [Test]
        public void BasicCursive_With_InconsistentCursive()
        {
            Markdown.Markdown.Render("Text _with_ text _ab")
                .Should().Be(@"Text <em>with<\em> text _ab");
        }

        [Test]
        public void AllLine_Is_Cursive()
        {
            Markdown.Markdown.Render("_abbbac_")
                .Should().Be(@"<em>abbbac<\em>");
        }

        [Test]
        public void Digits_In_Cursive()
        {
            Markdown.Markdown.Render("Text _abc 1 abc_ text")
                .Should().Be(@"Text _abc 1 abc_ text");
        }


        [Test]
        public void Cursive_In_Different_Words_Parts()
        {
            Markdown.Markdown.Render("Tex_t te_xt")
                .Should().Be(@"Tex_t te_xt");
        }

        [Test]
        public void Cursive_In_Word_Beginning()
        {
            Markdown.Markdown.Render("abc _te_xt")
                .Should().Be(@"abc <em>te<\em>xt");
            Markdown.Markdown.Render("_te_xt")
                .Should().Be(@"<em>te<\em>xt");
        }

        [Test]
        public void Cursive_In_Word_Ending()
        {
            Markdown.Markdown.Render("te_xt_")
                .Should().Be(@"te<em>xt<\em>");
            Markdown.Markdown.Render("te_xt_ abc")
                .Should().Be(@"te<em>xt<\em> abc");
        }

        [Test]
        public void Cursive_In_Word_Middle()
        {
            Markdown.Markdown.Render("t_ex_t")
                .Should().Be(@"t<em>ex<\em>t");
        }
    }
}