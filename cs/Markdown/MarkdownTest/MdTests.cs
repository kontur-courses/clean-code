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

        [Test]
        public void SingleHeaderTest_ShouldWork()
        {
            var text =
                @"# Header
other text";
            md.GetHtmlMarkup(text).Should().Be(@"<h1> Header</h1>
other text");
        }

        [Test]
        public void SingleItalicTag_ShouldWork()
        {
            var text = @"some text _italic text_ not italic text";

            md.GetHtmlMarkup(text).Should().Be(@"some text <em>italic text</em> not italic text");
        }

        [Test]
        public void SingleBoldTag_ShouldWork()
        {
            var text = @"some text __bold text__ not bold text";

            md.GetHtmlMarkup(text).Should().Be(@"some text <strong>bold text</strong> not bold text");
        }

        [Test]
        public void Tags_ShouldBeInCorrectOrder()
        {
            var text = @"abc # _dsa_";

            md.GetHtmlMarkup(text).Should().Be(@"abc <h1><em>dsa</em></h1>");
        }
    }
}