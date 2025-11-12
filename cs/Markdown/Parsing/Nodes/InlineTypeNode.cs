using System.Text;

namespace Markdown.Parsing.Nodes;

public abstract class InlineTypeNode : INode
{
    public abstract void RenderHtml(StringBuilder sb);
}