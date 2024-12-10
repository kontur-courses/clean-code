using Markdown.Enums;

namespace Markdown.AstNodes;

public class BoldMarkdownNode : MarkdownNode, IMarkdownNodeWithChildren
{
    public override MarkdownNodeName Type => MarkdownNodeName.Bold;
    public List<MarkdownNode> Children { get; } = [];
}