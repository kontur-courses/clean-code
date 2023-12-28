using Markdown;

namespace MarkdownTests;

public class HeadlineTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokens = new ParseTokens();
        md = new Md(tokens, converter);
    }
    
    [TestCase(@"# Report", @"<h1> Report</h1>", TestName = "Converted headline when when it is written correctly")]
    [TestCase(@"\#Report", @"#Report", TestName = "It is not converted headline when escaping character")]
    [TestCase(@"\\_Report_", @"\<em>Report</em>", TestName = "Converted cursive when two escaping character")]
    public void RenderHeadline(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }
}