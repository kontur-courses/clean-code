namespace MarkdownTests;

[TestFixture]
public class HeadlineTests
{

    [Test]
    public void Render_ShouldHeadline()
    {
        Markdown.Md.Render("#Report")
            .Should().Be(@"<h1>Report</h1>");
    }
    
    [Test]
    public void Render_ShouldNotHeadline_WhenEscaped()
    {
        Markdown.Md.Render(@"\#Report")
            .Should().Be(@"#Report");
    }

    [Test]
    public void Render_ShouldCursiveAndBold_WhenInHeadline()
    {
        Markdown.Md.Render("#Report _to_ __word__")
            .Should().Be(@"<h1>Report <em>to</em> <strong>word</strong></h1>");
    }

    [Test]
    public void Render_ShouldSharpSymbol_WhenNotTheFirst()
    {
        Markdown.Md.Render("#Report #")
            .Should().Be(@"<h1>Report #</h1>");
    }

    [Test]
    public void Render_ShouldSlash_WhenHeadlineHasAlreadyBeenUsed()
    {
        Markdown.Md.Render(@"#Report \#")
            .Should().Be(@"<h1>Report \#</h1>");
    }
}