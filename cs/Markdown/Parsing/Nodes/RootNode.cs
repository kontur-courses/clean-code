using System.Text;

namespace Markdown.Parsing.Nodes;

public class RootNode(List<BlockTypeNode> blocks) : INode
{
    public List<BlockTypeNode> Blocks { get; } = blocks;

    public void RenderHtml(StringBuilder sb)
    {
        foreach (var block in Blocks)
            block.RenderHtml(sb);
    }
}