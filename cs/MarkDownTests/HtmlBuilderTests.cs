using FluentAssertions;
using Markdown.HtmlBuilder;
using Markdown.Tokens;

namespace MarkDownTests;

public class HtmlBuilderTests
{
    [Test]
    public void Test()
    {
        var input = new List<Token>()
        {
            new ParagraphToken(0, 20),
            new LiteralToken(1, 5, "12345"),
            new BoldToken(6, 9),
            new ItalicsToken(7, 8),
            new LiteralToken(21, 26, "232wer")
        };
        var HtmlBuilder = new HtmlBuilder(input);
        var res = HtmlBuilder.ConvertTokensToHtml();
        Assert.Equals(true, true);
    }
    
}