using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class HeadlineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Render_ShouldRenderHeadline()
        {
            Markdown.Markdown.Render("#Text")
                .Should().Be(@"<h1>Text<\h1>");
        }

        [Test]
        public void Render_ShouldRenderCursiveAndBold_WhenInHeadline()
        {
            Markdown.Markdown.Render("#Text _with_ __text__")
                .Should().Be(@"<h1>Text <em>with<\em> <strong>text<\strong><\h1>");
        }

        [Test]
        public void Render_ShouldRenderSharpSymbol_WhenNotLeading()
        {
            Markdown.Markdown.Render("#Text #")
                .Should().Be(@"<h1>Text #<\h1>");
        }

        [Test]
        public void Render_ShouldRenderSlash_WhenBeforeNotLeadingSharp()
        {
            Markdown.Markdown.Render(@"#Text \#")
                .Should().Be(@"<h1>Text \#<\h1>");
        }

        [Test]
        public void Render_ShouldNotRenderHeadlineAndSlash_WhenLeadingSharpIsEscaped()
        {
            Markdown.Markdown.Render(@"\#Text")
                .Should().Be(@"#Text");
        }
    }
}