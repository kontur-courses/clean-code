using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class MdTests
    {
        private Md md = new Md();

        [Test]
        public void Should_MakeEmTag_WhenOneWordSurroundedByUnderscores()
        {
            var text = "_курсив_";
            md.Render(text).Should().Be("<em>курсив</em>");
        }

        [Test]
        public void Should_MakeEmTag_WhenWordsSurroundedByUnderscores()
        {
            var text = "_и здесь курсив_";
            md.Render(text).Should().Be("<em>и здесь курсив</em>");
        }

        [Test]
        public void Should_MakeStrongTag_WhenOneWordSurroundedByUnderscores()
        {
            var text = "__жирный__";
            md.Render(text).Should().Be("<strong>жирный</strong>");
        }

        [Test]
        public void Should_MakeStrongTag_WhenWordsSurroundedByUnderscores()
        {
            var text = "__и здесь жирный__";
            md.Render(text).Should().Be("<strong>и здесь жирный</strong>");
        }

        [Test]
        public void Should_MakeEmTagInsideStrongTag_WhenDoubleUnderscoresHaveInnerUnderscores()
        {
            var text = "__просто жирный _жирный курсив___";
            md.Render(text).Should().Be("<strong>просто жирный <em>жирный курсив</em></strong>");
        }
    }
}
