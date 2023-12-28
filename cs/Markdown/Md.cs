using System.Text;

namespace Markdown;

public class Md
{
    private ParseTokens tokens;
    private HtmlConverter converter;
    public Md(ParseTokens tokens, HtmlConverter converter)
    {
        this.tokens = tokens;
        this.converter = converter;
    }

    public string Render(string markdownText)
    {
        var tokens = this.tokens.ParserTokens(markdownText);
        return converter.ConvertMarkdownToHtml(markdownText, tokens);
    }
}
