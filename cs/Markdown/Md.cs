using Markdown.Abstractions;
using Markdown.Parsers;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    private static readonly ITextParser MarkdownParser = new MarkdownParser();
    private static readonly IMarkedTextRenderer HtmlRenderer = new HtmlRenderer();
    public static string Render(string markdownText)
    {
        var tokens = MarkdownParser.Parse(markdownText);
        
        return HtmlRenderer.Render(tokens);
    }
}