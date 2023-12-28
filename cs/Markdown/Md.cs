namespace Markdown;

public class Md
{
    public string Render(string markdownText)
    {
        return new TokenConverter().TokensToString(new TokenHighlighter().HighlightTokens(markdownText));
    }
}