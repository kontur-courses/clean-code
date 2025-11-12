using System.Text;
using Markdown.Renderers;

namespace Markdown;

public class Md
{
    public string Render(string markdown)
    {
        var mdToHtml = new MarkdownToHtmlRenderer();
        return mdToHtml.RenderWithConstraints(markdown, true);
    }
}