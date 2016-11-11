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
    }
}