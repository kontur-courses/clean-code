using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture]
    public class MdTests
    {
        [Test]
        public void WhenItalicTagInsideBoldTag_ShouldReturnTwoTags()
        {
            Md md = new Md();

            var actual = md.Render("__двойного выделения _одинарное_ тоже__");

            actual.Should().Be("<strong>двойного выделения <em>одинарное</em> тоже</strong>");
        }

        [Test]
        public void WhenPassUnpairedSymbols_ShouldNotChangAnything()
        {
            Md md = new Md();

            var actual = md.Render("__разные выделения_");

            actual.Should().Be("__разные выделения_");
        }
    }
}
