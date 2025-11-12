using System;
using System.Text;
using Markdown.Parsing.Nodes;

namespace Markdown.Rendering;

public static class HtmlRenderer
{
    public static string Render(RootNode document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var sb = new StringBuilder();
        document.RenderHtml(sb);
        return sb.ToString();
    }

    public static string EscapeHtml(string? text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var sb = new StringBuilder(text.Length);
        foreach (var c in text)
        {
            switch (c)
            {
                case '&': sb.Append("&amp;"); break;
                case '<': sb.Append("&lt;"); break;
                case '>': sb.Append("&gt;"); break;
                case '"': sb.Append("&quot;"); break;
                case '\'': sb.Append("&#39;"); break;
                default: sb.Append(c); break;
            }
        }

        return sb.ToString();
    }
}