namespace Markdown;

public class Md
{
    private readonly IParser parser = new MarkdownParser();
    private readonly IRenderer renderer = new HtmlRenderer();
    
    public string Render(string markdownText)
    {
        var tokens = parser.Parse(markdownText);
        var result = renderer.Render(tokens, markdownText);
        return result;
    }
}