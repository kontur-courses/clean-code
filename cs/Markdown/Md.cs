using Markdown.Interfaces;

namespace Markdown;

public class Md
{
    private readonly IRenderer renderer;
    private readonly IMarkdownParser parser;
    
    public Md(IRenderer renderer, IMarkdownParser parser)
    {
        this.renderer = renderer;
        this.parser = parser;
    }

    public string Render(string markdownText)
    {
        var tokens = parser.ParseTokens(markdownText);
        return renderer.Render(tokens);
    }
}