using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdRenderTest
    {
        public Md Md;
        [SetUp]
        public void SetUp()
        {
            Md = new Md();
        }
        
        [Test]
        public void Render_CorrectWork_OnNotStyledText()
        {
            Md.Render(new string[] {"asdf", "asdf"}).Should().Be("asdfasdf");
        }

        [Test]
        public void Render_CorrectWork_WithHeaderStrings()
        {
            Md.Render(new[] {"asdf", "#asdf"}).Should().Be("asdf<h1>asdf</h1>");
            
        }

        [Test]
        public void Render_CorrectWork_WithItalicToken()
        {
            Md.Render(new[] {"_asdf_"}).Should().Be("<em>asdf</em>");
        }

        [Test]
        public void Render_CorrectWork_WithBoldToken()
        {
            Md.Render(new[] {"__asdf__"}).Should().Be("<strong>asdf</strong>");
        }

        [Test]
        public void Render_CorrectWork_OnItalicTokenInBold()
        {
            Md.Render(new[] {"__one_two_three__"}).Should()
                .Be("<strong>one<em>two</em>three</strong>");
        }
    }
}