using NUnit.Framework;
using Markdown;
using FluentAssertions;

namespace MarkdownTest
{
    [TestFixture]
    public class BoldTests
    {
        [Test]
        public void WhenPassBoldSymbol_ShouldConvertToStrongHtml()
        {
            Md md = new Md();

            var actual = md.Render("__Test__");

            actual.Should().Be("<strong>Test</strong>");
        }
    }
}
