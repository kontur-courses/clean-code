using FluentAssertions;
using Markdown.Shells;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class SingleUnderline_Should
    {
        private SingleUnderline singleUnderline;

        [SetUp]
        public void SetUp()
        {
            singleUnderline = new SingleUnderline();
        }

        [Test]
        public void HavePrefixSingleUnderline()
        {
            singleUnderline.GetPrefix().Should().Be("_");
        }
        [Test]
        public void HaveSuffixSingleUnderline()
        {
            singleUnderline.GetSuffix().Should().Be("_");
        }

        [Test]
        public void RenderTextToHtml()
        {
            singleUnderline.RenderToHtml("some text").Should().Be("<em>some text</em>");
        }
        [Test]
        public void NotContainsDoubleUnderline()
        {
            singleUnderline.Contains(new DoubleUnderline()).Should().BeFalse();
        }
    }
}
