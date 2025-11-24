using System.Text;

namespace Markdown.Domains.Nodes;

public class RootNode(List<Node>? children = null) : Node(children)
{
    public override void ConvertToHtml(StringBuilder sb)
    {
        RenderChildren(sb);
    }

    public string ToHtml()
    {
        var builder = new StringBuilder();
        ConvertToHtml(builder);
        return builder.ToString();
    }
}