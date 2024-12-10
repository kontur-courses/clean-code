using Markdown.Enums;

namespace Markdown.AstNodes;

public class HeadingMarkdownNode : MarkdownNode, IMarkdownNodeWithChildren
{
    public override MarkdownNodeName Type => MarkdownNodeName.Heading;
    public List<MarkdownNode> Children { get; } = [];
}