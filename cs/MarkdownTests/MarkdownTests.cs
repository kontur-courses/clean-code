using Markdown;
using NUnit.Framework;
using System.Collections.Generic;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTests
    {
        private Md md;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            md = new Md();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Render_DoesNotThrow_OnNullOrWhitespace(string mdText)
        {
            Assert.DoesNotThrow(() => md.Render(mdText));
        }

        [TestCase(@"\_text\_", ExpectedResult = @"_text_")]
        [TestCase(@"\__text\__", ExpectedResult = @"_<em>text_</em>")]
        [TestCase(@"\text text", ExpectedResult = @"\text text")]
        [TestCase(@"text \ text", ExpectedResult = @"text \ text")]
        [TestCase(@"text text \", ExpectedResult = @"text text \")]
        public string Render_ShouldCorrectRenderDisablingSymbol(string mdText)
        {
            return md.Render(mdText);
        }

        [TestCase(Style.Italic, "text _italic", 5, ExpectedResult = true)]
        [TestCase(Style.Bold, "text __bold", 5, ExpectedResult = true)]
        [TestCase(Style.Italic, "text ", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "text %", 5, ExpectedResult = false)]
        public bool Style_IsTag_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.IsTag(ref text, index);
        }

        [TestCase(Style.Italic, "text_", 4, ExpectedResult = true)]
        [TestCase(Style.Italic, "text _", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "t00t_", 4, ExpectedResult = false)]
        public bool Style_CanEnd_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.CanBegin(ref text, index, new Stack<(Style style, int endIndex)>(), out int _);
        }

        [TestCase(Style.Italic, "text _italic_", 5, ExpectedResult = true)]
        [TestCase(Style.Bold, "text __bold__", 5, ExpectedResult = true)]
        [TestCase(Style.Italic, "text ", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "text _", 5, ExpectedResult = false)]
        [TestCase(Style.Italic, "_t00t_", 0, ExpectedResult = false)]
        public bool Style_CanBegin_ReturnsCorrectValue(Style style, string text, int index)
        {
            return style.CanBegin(ref text, index, new Stack<(Style style, int endIndex)>(), out int _);
        }
    }
}
