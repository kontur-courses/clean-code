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
    }
}