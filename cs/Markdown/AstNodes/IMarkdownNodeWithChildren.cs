namespace Markdown.AstNodes;

public interface IMarkdownNodeWithChildren
{
    public List<MarkdownNode> Children { get; }
}