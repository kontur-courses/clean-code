using Markdown.Parsers;
using Markdown.Renderers;

namespace Markdown;

public sealed class Md(
    MarkdownParser parser,
    HtmlRenderer renderer)
{
    public string Render(string markdownText)
    {
        var markdownBlocks = parser.Parse(markdownText);
        
        return renderer.Render(markdownBlocks);
    }
}