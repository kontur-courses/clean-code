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
    }
}