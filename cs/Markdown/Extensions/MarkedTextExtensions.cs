using Markdown.Abstractions;
using Markdown.Models;
using Markdown.Renderers;

namespace Markdown.Extensions;

public static class MarkedTextExtensions
{
    public static string Render(this MarkedText markedText, IMarkedTextRenderer renderer)
    {
        return renderer.Render(markedText);
    }

    public static string RenderHtml(this MarkedText markedText)
    {
        return markedText.Render(new HtmlRenderer());
    }
}