using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class BoldTests
    {
        [Test]
        public void BasicBold()
        {
            Markdown.Markdown.Render("Text __with__ text")
                .Should().Be(@"Text <strong>with<\strong> text");
        }

        [Test]
        public void BasicBold_With_InconsistentBold()
        {
            Markdown.Markdown.Render("Text __with__ text __ab")
                .Should().Be(@"Text <strong>with<\strong> text __ab");
        }

        [Test]
        public void AllLine_Is_Bold()
        {
            Markdown.Markdown.Render("__abbbac__")
                .Should().Be(@"<strong>abbbac<\strong>");
        }

        [Test]
        public void Digits_In_Bold()
        {
            Markdown.Markdown.Render("Text __abc 1 abc__ Text")
                .Should().Be(@"Text __abc 1 abc__ Text");
        }
    }
}