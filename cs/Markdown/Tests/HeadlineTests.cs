namespace Markdown.Tests;

[TestFixture]
internal class HeadlineTests
{

    [Test]
    public void Render_ShouldRenderHeadline()
    {
        Markdown.Md.Render("#Text")
            .Should().Be(@"<h1>Text<\h1>");
    }

    [Test]
    public void Render_ShouldRenderCursiveAndBold_WhenInHeadline()
    {
        Markdown.Md.Render("#Text _with_ __text__")
            .Should().Be(@"<h1>Text <em>with<\em> <strong>text<\strong><\h1>");
    }

    [Test]
    public void Render_ShouldRenderSharpSymbol_WhenNotLeading()
    {
        Markdown.Md.Render("#Text #")
            .Should().Be(@"<h1>Text #<\h1>");
    }

    [Test]
    public void Render_ShouldRenderSlash_WhenBeforeNotLeadingSharp()
    {
        Markdown.Md.Render(@"#Text \#")
            .Should().Be(@"<h1>Text \#<\h1>");
    }

    [Test]
    public void Render_ShouldNotRenderHeadlineAndSlash_WhenLeadingSharpIsEscaped()
    {
        Markdown.Md.Render(@"\#Text")
            .Should().Be(@"#Text");
    }
}