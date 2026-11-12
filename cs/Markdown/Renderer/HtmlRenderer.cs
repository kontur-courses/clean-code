using Markdown.Node;
using Markdown.Renderer.Interfaces;
using Markdown.Node.Type;
using System.Text;
using Markdown.Node.Interfaces;
using System.Net;

namespace Markdown.Renderer;

public class HtmlRenderer : IRenderer
{
    public string Render(IReadOnlyList<BlockNode> blocks)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < blocks.Count; i++)
        {
            if (i > 0)
                sb.Append('\n');

            RenderBlock(blocks[i], sb);
        }

        return sb.ToString();
    }

    private void RenderBlock(BlockNode block, StringBuilder sb)
    {
        if (block.BlockType == BlockType.Header1)
        {
            sb.Append("<h1>");
            RenderNodes(block.Children, sb);
            sb.Append("</h1>");
        }
        else
        {
            RenderNodes(block.Children, sb);
        }
    }

    private void RenderNodes(IReadOnlyCollection<INode> nodes, StringBuilder sb)
    {
        foreach (var node in nodes)
            RenderNode(node, sb);
    }

    private void RenderNode(INode node, StringBuilder sb)
    {
        switch (node)
        {
            case TextNode textNode:
                sb.Append(WebUtility.HtmlEncode(textNode.Text));
                break;

            case EmphasisNode emphasisNode:
                var tag = emphasisNode.EmphasisType == EmphasisType.Italic ? "em" : "strong";
                sb.Append($"<{tag}>");
                RenderNodes(emphasisNode.Children, sb);
                sb.Append($"</{tag}>");
                break;

            case LinkNode linkNode:
                sb.Append($"<a href=\"{WebUtility.HtmlEncode(linkNode.Url)}\">");
                RenderNodes(linkNode.Children, sb);
                sb.Append("</a>");
                break;

            default:
                throw new ArgumentOutOfRangeException($"Unknown node type: {node.GetType().Name}");
        }
    }
}