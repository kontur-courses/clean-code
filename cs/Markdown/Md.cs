using Markdown.Parser.Interfaces;
using Markdown.Renderer.Interfaces;

namespace Markdown;

public class Md
{
    private readonly IMarkdownParser _markdownParser;
    private readonly IHtmlRenderer _htmlRenderer;

    public Md(IMarkdownParser markdownParser, IHtmlRenderer htmlRenderer)
    {
        _markdownParser = markdownParser;
        _htmlRenderer = htmlRenderer;
    }

    public string Render(string markdown)
    {
        throw new NotImplementedException();
    }
}
