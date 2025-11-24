using System.Text;

namespace Markdown.Domains.Nodes;

public class NewLineNode : Node
{
    public override void ConvertToHtml(StringBuilder sb)
    {
        sb.Append("<br/>");
    }
}