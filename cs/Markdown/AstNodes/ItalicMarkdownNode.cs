using Markdown.Enums;

namespace Markdown.AstNodes;

public class ItalicMarkdownNode : MarkdownNode, IMarkdownNodeWithChildren
{
    public override MarkdownNodeName Type => MarkdownNodeName.Italic;
    public List<MarkdownNode> Children { get; } = [];
}