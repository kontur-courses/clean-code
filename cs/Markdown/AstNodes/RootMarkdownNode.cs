using Markdown.Enums;

namespace Markdown.AstNodes;

public class RootMarkdownNode : MarkdownNode, IMarkdownNodeWithChildren
{
    public override MarkdownNodeName Type => MarkdownNodeName.Root;
    public List<MarkdownNode> Children { get; } = [];
}