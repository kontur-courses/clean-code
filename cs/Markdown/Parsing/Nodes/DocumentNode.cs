namespace Markdown.Parsing.Nodes;

public class DocumentNode : MarkdownNode
{
    public List<MarkdownNode> Children { get; set; } = [];

    public override string ToHtml() => string.Concat(Children.Select(child => child.ToHtml()));
}