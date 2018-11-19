using NUnit.Framework;
using  FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTests
    {
        public Markdown render; 

        [SetUp]
        public void SetUp()
        {
            render = new Markdown();
        }

        [Test]
        public void Render_OrdinaryText_ReturnExactText()
        {
            var mdText = "Simple text.";
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(mdText);
        }

        [Test]
        public void Render_EmptyString_EmptyString()
        {
            var mdText = "";
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(mdText);
        }

        [TestCase("_look at this_", "<em>look at this</em>", TestName = "Italic text")]
        [TestCase("__look at__ this", "<strong>look at</strong> this", TestName = "Bold text")]
        [TestCase("look `at this`", "look <code>at this</code>", TestName = "Text with quote")]
        public void Render_OneTypeOfSymbols_CorrectHTMLText(string mdText, string expectedHtmlText)
        {
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }

        [TestCase("_look `at this`_", "<em>look <code>at this</code></em>", TestName = "Italic and Code")]
        [TestCase("__look _at_ this__", "<strong>look <em>at</em> this</strong>", TestName = "Bold and Italic")]
        [TestCase("`look __at__ this`", "<code>look <strong>at</strong> this</code>", TestName = "Code And Bold")]
        public void Render_SeveralTypesOfSymbols_CorrectHTMLText(string mdText, string expectedHtmlText)
        {
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }

        [TestCase("\\_Si txt\\__", "_Si txt__")]
        [TestCase("Si\\` txt\\_\\__\\`", "Si` txt___`")]
        public void Render_HaveBackslash_ShieldSymbols(string mdText, string expectedHtmlText)
        {
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }

        [TestCase("_hello __its__ me_", "<em>hello __its__ me</em>")]
        public void Render_HaveBoldTextBetweenItalic_ShieldingSymbols(string mdText, string expectedHtmlText)
        {
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }

        [Test]
        public void bigtest()
        {
            var mdText = @"\__lala `la` _la __lala__ la_";
            var expectedHtmlText = "__lala <code>la</code> <em>la __lala__ la</em>";
            var htmlText = render.Render(mdText);
            htmlText.Should().Be(expectedHtmlText);
        }
    }
}
