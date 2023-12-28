using Markdown;

namespace MarkdownTests;

public class EscapeTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokens = new ParseTokens();
        md = new Md(tokens, converter);
    }
    
    [TestCase(@"\# \\ \_\_\_ \_", @"# \ ___ _", TestName = "Converted escaping character")]
    [TestCase(@"\# Report", @"# Report", TestName = "Converted headline markdown when escaping character")]
    [TestCase(@"I \_have 1 bread_", @"I _have 1 bread_", TestName = "Converted cursive markdown when escaping character")]
    public void RenderExcape(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }
}