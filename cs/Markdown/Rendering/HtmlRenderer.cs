using System.Text;
using Markdown.Blocks;
using Markdown.Tree;

namespace Markdown.Rendering;

public class HtmlRenderer
{
    public string Render(BlockType blockType, IReadOnlyList<INode> nodes)
    {
        throw new NotImplementedException();
    }

    private static void RenderNode(INode node, StringBuilder sb)
    {
        throw new NotImplementedException();
    }

    private static void RenderWrapped(string tag, IReadOnlyList<INode> children, StringBuilder sb)
    {
        throw new NotImplementedException();
    }

    private static string EscapeAttribute(string value)
    {
        throw new NotImplementedException();
    }
}