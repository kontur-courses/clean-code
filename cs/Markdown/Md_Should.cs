using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        private Md md;

        [Test]
        public void RenderOriginalText()
        {
            md.Render("asd").Should().Be("asd");
        }

        [Test]
        public void RenderItalicTag()
        {
            md.Render("_asd_").Should().Be("<em>asd</em>");
        }

        [Test]
        public void RenderBoldTag()
        {
            md.Render("__asd__").Should().Be("<strong>asd</strong>");
        }

        [Test]
        public void RenderHeaderTag()
        {
            md.Render("# asd").Should().Be("<h1>asd</h1>");
        }

        [Test]
        public void NotRenderItalicTag_WhenEscaped()
        {
            md.Render("/_asd/_").Should().Be("_asd_");
        }

        [Test]
        public void RenderItalicTag_WhenEscapeSymbolEscaped()
        {
            md.Render("//_asd//_").Should().Be("/<em>asd/</em>");
        }

        [Test]
        public void RenderItalicTag_InWordBeginning()
        {
            md.Render("_as_d").Should().Be("<em>as</em>d");
        }

        [Test]
        public void RenderItalicTag_InWordEnding()
        {
            md.Render("a_sd_").Should().Be("a<em>sd</em>");
        }

        [Test]
        public void RenderItalicTag_InWordMiddle()
        {
            md.Render("a_s_d").Should().Be("a<em>s</em>d");
        }

        [Test]
        public void NotRenderItalicTag_WhenTagsInMiddleDifferentWords()
        {
            md.Render("as_d a_sd").Should().Be("as_d a_sd");
        }
        
        [Test]
        public void NotRenderTags_WhenTagsIntersecting()
        {
            md.Render("__as_d __a_sd").Should().Be("__as_d __a_sd");
        }
        
        [Test]
        public void NotRenderBoldTag_WhenNestingInItalicTag()
        {
            md.Render("_a__s__d_").Should().Be("<em>a__s__d</em>");
        }
    }
}