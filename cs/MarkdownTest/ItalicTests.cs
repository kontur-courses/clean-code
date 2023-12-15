using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class ItalicTests
    {
        [Test]
        public void WhenPassItalicSymbol_ShouldConvertToEmHtml()
        {
            Md md = new Md();

            var actual = md.Render("_Test_");

            actual.Should().Be("<em>Test</em>");
        }

        [Test]
        public void WhenItalicsInPartOfWord_ShouldConvertToEmHtml()
        {
            Md md = new Md();

            var actual = md.Render("_нач_але");

            actual.Should().Be("<em>нач</em>але");
        }
    }
}
