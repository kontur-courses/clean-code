namespace Markdown.Tests;

[TestFixture]
internal class EscapeSymbolTests
{
    [Test]
    public void Render_ShouldRenderEscapableSymbol_WhenEscaped()
    {
        Markdown.Md.Render(@"\# \\ \_ \_\_")
            .Should().Be(@"# \ _ __");
    }

    [Test]
    public void Render_ShouldRenderWrap_WhenContainsEscapedSymbols()
    {
        Markdown.Md.Render(@"#\# _a \_ b_ __a \_\_ b__")
            .Should().Be(@"<h1>\# <em>a _ b<\em> <strong>a __ b<\strong><\h1>");
    }
}