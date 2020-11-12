using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdRenderTest
    {
        [Test]
        public void Render_CorrectWork_OnNotStyledText()
        {
            var md = new Md();
            md.Render(new string[] {"asdf", "asdf"}).Should().Be("asdfasdf");
        }

        [Test]
        public void Render_CorrectWork_WithHeaderStrings()
        {
            var md = new Md();
            md.Render(new[] {"asdf", "#asdf"}).Should().Be("asdf<h1>asdf</h1>");
            
        }
    }
}