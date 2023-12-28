using Markdown;

namespace MarkdownTests;

[TestFixture]
public class BoldTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokenSearcher = new ParseTokens();
        md = new Md(tokenSearcher, converter);
    }
    
    [TestCase("Hello __Peter__ ?", @"Hello <strong>Peter</strong> ?", TestName = "Converted bold when when it is written correctly")]
    [TestCase("I __go to home__ when __cold", @"I <strong>go to home</strong> when __cold", TestName = "Converted bold when pair bold is full")]
    [TestCase("I __have 1 bread__", @"I <strong>have 1 bread</strong>", TestName = "Converted bold when the tag is not in the number")]
    [TestCase("I have1__2 bread__", @"I have1__2 bread__", TestName = "It is not converted bold when the tag is in the number")]
    public void RenderBold(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }
}