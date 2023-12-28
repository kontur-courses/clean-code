using Markdown;

namespace MarkdownTests;

[TestFixture]
public class CursiveTests
{
    private Md md;
    
    [SetUp]
    public void Init()
    {
        var converter = new HtmlConverter();
        var tokens = new ParseTokens();
        md = new Md(tokens, converter);
    }
    
    [TestCase("I _go_ to _home", @"I <em>go</em> to _home", TestName = "Converted cursive when only pair is full")]
    [TestCase("_home_", @"<em>home</em>", TestName = "Converted cursive when it is written correctly")]
    [TestCase("I _write 1 digit_ in text", @"I <em>write 1 digit</em> in text", TestName = "Converted cursive when the tag is not in the number")]
    [TestCase("I have1_2 bread_", @"I have1_2 bread_", TestName = "It is not converted cursive when the tag is in the number")]
    [TestCase("Hom_e h_ot", @"Hom_e h_ot", TestName = "It is not converted cursive when tag in different words")]
    [TestCase("No _so_lid", @"No <em>so</em>lid", TestName = "Converted cursive when in different parts of same word")]
    public void Render_Cursive(string markdownText, string htmlText)
    {
        md.Render(markdownText).Should().Be(htmlText);
    }
}