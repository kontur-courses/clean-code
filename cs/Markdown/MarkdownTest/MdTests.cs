using FluentAssertions;
using Markdown;
using NUnit.Framework;


namespace MarkdownTest
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void Setup()
        {
            md = new Md();
        }

        [Test]
        public void ReturnsText_WhenThereIsNoMarkup()
        {
            var text = "123 dsa 542 asfaasfg";
            md.GetHtmlMarkup(text).Should().Be(text);
        }
    }
}