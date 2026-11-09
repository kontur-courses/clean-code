using Markdown.Parsing;
using Markdown.Rendering;

namespace Markdown;

public class MarkdownConverter
{
    public static string ToHtml(string markdown)
    {
        var nodes = NodeBuilder.Build(markdown);

        return HtmlRenderer.Render(nodes);
    }
}