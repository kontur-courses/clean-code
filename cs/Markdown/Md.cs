namespace Markdown;

public class Md
{
    public string Render(string markdownText)
    {
        var tokens = new TokenHighlighter().HighlightTokens(markdownText);

        return new TokenConverter().TokensToString(tokens);
    }
}