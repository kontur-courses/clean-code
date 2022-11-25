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

        [Test]
        public void Bold_In_Different_Words_Parts()
        {
            Markdown.Markdown.Render("Tex__t te__xt")
                .Should().Be(@"Tex__t te__xt");
        }

        [Test]
        public void Bold_In_Word_Beginning()
        {
            Markdown.Markdown.Render("abc __te__xt")
                .Should().Be(@"abc <strong>te<\strong>xt");
            Markdown.Markdown.Render("__te__xt")
                .Should().Be(@"<strong>te<\strong>xt");
        }

        [Test]
        public void Bold_In_Word_Ending()
        {
            Markdown.Markdown.Render("te__xt__")
                .Should().Be(@"te<strong>xt<\strong>");
            Markdown.Markdown.Render("te__xt__ abc")
                .Should().Be(@"te<strong>xt<\strong> abc");
        }

        [Test]
        public void Bold_In_Word_Middle()
        {
            Markdown.Markdown.Render("t__ex__t")
                .Should().Be(@"t<strong>ex<\strong>t");
        }
    }
}