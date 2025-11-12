using System.Text;

namespace Markdown.Parsing.Nodes;

public abstract class BlockTypeNode : INode
{
    public abstract void RenderHtml(StringBuilder sb);
}