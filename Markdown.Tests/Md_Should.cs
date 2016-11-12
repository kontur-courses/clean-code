using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void notСhangeText_WhenNoFormatting()
        {
            var text = "text without formatting";
            md.Render(text).Should().Be(text);
        }

        [Test]
        public void addItalicTag_WhenIsSingleUnderline()
        {
            var text = "_italic text_";
            md.Render(text).Should().Be("<em>italic text</em>");
        }

        [Test]
        public void addBoldTag_WhenIsDoubleUnderline()
        {
            var text = "__bold text__";
            md.Render(text).Should().Be("<strong>bold text</strong>");
        }

        [Test]
        public void renderItalic_WhenInsideBoldTag()
        {
            var text = "__bold _italic and bold_ bold__";
            md.Render(text).Should().Be("<strong>bold <em>italic and bold</em> bold</strong>");
        }

        [Test]
        public void notRenderBold_WhenInsideItalicTag()
        {
            var text = "_italic __not bold__ italic_";
            md.Render(text).Should().Be("<em>italic _</em>not bold__ italic_");
        }

        [Test]
        public void notFindShell_WhenSurroundedByNumbers()
        {
            var text = "_italic2_0italic_";
            md.Render(text).Should().Be("<em>italic2_0italic</em>");
        }

        [TestCase("\\_simple text\\_", ExpectedResult = "_simple text_")]
        [TestCase("_italic\\_ text_", ExpectedResult = "<em>italic_ text</em>")]
        public string removeShieldingCharacters_AfterRender(string text)
        {
            return md.Render(text);
        }


    }
}
