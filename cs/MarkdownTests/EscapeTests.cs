namespace MarkdownTests;

[TestFixture]
public class EscapeTests
{
    [Test]
    public void Render_ShouldEscapableSymbol_WhenEscaped()
    {
        Markdown.Md.Render(@"\# \\ \_\_\_ \_")
            .Should().Be(@"# \ ___ _");
    }

    [Test]
    public void Render_ShouldBoldAndEscaped_WhenPairIsFullAndHaveEscaped()
    {
        Markdown.Md.Render(@"#\_\_ _me \_ go_ __to \_\_ home__")
            .Should().Be(@"<h1>__ <em>me _ go</em> <strong>to __ home</strong></h1>");
    }
}