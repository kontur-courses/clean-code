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
    }
}