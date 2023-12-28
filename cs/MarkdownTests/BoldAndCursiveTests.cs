using Markdown;

namespace MarkdownTests;

[TestFixture]
public class BoldAndCursiveTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokenSearcher = new ParseTokens();
        md = new Md(tokenSearcher, converter);
    }
    
    [TestCase("I _go __in_ home__", @"I _go __in_ home__", TestName = "It is not converted cursive and bold when intersect")]
    [TestCase("Me _eat __cool__ meat_", @"Me <em>eat __cool__ meat</em>", TestName = "It is not converted bold when bold in cursive")]
    [TestCase("I __go _to_ home__", @"I <strong>go <em>to</em> home</strong>", TestName = "Converted when cursive in bold")]
    public void Render_BoldAndCursive(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }
}