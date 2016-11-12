using NUnit.Framework;
using FluentAssertions;
using Markdown.Shells;

namespace Markdown.Tests
{
    [TestFixture]
    public class DoubleUnderline_Should
    {
        private DoubleUnderline doubleUnderline;

        [SetUp]
        public void SetUp()
        {
            doubleUnderline = new DoubleUnderline();
        }

        [Test]
        public void HavePrefixDoubleUnderline()
        {
            doubleUnderline.GetPrefix().Should().Be("__");
        }
        [Test]
        public void HaveSuffixDoubleUnderline()
        {
            doubleUnderline.GetSuffix().Should().Be("__");
        }
        [Test]
        public void RenderTextToHtml()
        {
            doubleUnderline.RenderToHtml("some text").Should().Be("<strong>some text</strong>");
        }

        [Test]
        public void ContainsSingleUnderline()
        {
            doubleUnderline.Contains(new SingleUnderline()).Should().BeTrue();
        }
    }
}
