using Markdown;

namespace MarkdownTests;


[TestFixture]
public class MarkdonTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokenSearcher = new ParseTokens();
        md = new Md(tokenSearcher, converter);
    }
    
    [TestCase("Nikita", "Nikita", TestName = "Text no have tags")]
    [TestCase("Hello __Peter__ ?", @"Hello <strong>Peter</strong> ?", TestName = "Converted bold when when it is written correctly")]
    [TestCase("_Nikita_", @"<em>Nikita</em>", TestName = "Converted cursive when it is written correctly")]
    [TestCase("_Nikita_ __go__ in _home_", @"<em>Nikita</em> <strong>go</strong> in <em>home</em>", TestName = "Converted cursive and bold when it is written correctly")]
    [TestCase("# Report", "<h1> Report</h1>", TestName = "Converted headline when it is written correctly")]
    public void Render_Markdown(string markdownText, string htmlText)
    {
        md.Render(markdownText).Should().Be(htmlText);
    }
}