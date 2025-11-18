using System.Net;
using System.Text;

namespace Markdown.Parsing;

public interface INode
{
    public void Render(StringBuilder sb);

    public static void RenderNodes(List<INode> children, StringBuilder sb)
    {
        foreach (var node in children)
            node.Render(sb);
    }
}

public class DocumentNode(List<INode> children) : INode
{
    public List<INode> Children { get; } = children;

    public void Render(StringBuilder sb)
    {
        INode.RenderNodes(children, sb);
    }
}

public class TextNode(string text) : INode
{
    public string Text { get; } = text;
    public void Render(StringBuilder sb)
    {
        sb.Append(text);
    }
}

public class EmphasisNode(List<INode> children) : INode
{
    public List<INode> Children { get; } = children;
    public void Render(StringBuilder sb)
    {
        sb.Append("<em>");
        INode.RenderNodes(children, sb);
        sb.Append("</em>");
    }
}

public class StrongNode(List<INode> children) : INode
{
    public List<INode> Children { get; } = children;
    public void Render(StringBuilder sb)
    {
        sb.Append("<strong>");
        INode.RenderNodes(children, sb);
        sb.Append("</strong>");
    }
}

public class LinkNode(string href, List<INode> children) : INode
{
    public string Href { get; } = href;
    public List<INode> Children { get; } = children;
    public void Render(StringBuilder sb)
    {
        sb.Append("<a href=\"");
        sb.Append(WebUtility.HtmlEncode(href));
        sb.Append("\">");
        INode.RenderNodes(children, sb);
        sb.Append("</a>");
    }
}

public class HeadingNode(List<INode> children) : INode
{
    public List<INode> Children { get; } = children;
    public void Render(StringBuilder sb)
    {
        sb.Append("<h1>");
        INode.RenderNodes(children, sb);
        sb.Append("</h1>");
    }
}