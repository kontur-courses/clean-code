namespace Markdown.Parsing.Nodes;

public class BoldNode : MarkdownNode
{
    public List<MarkdownNode> Children { get; set; } = [];

    public override string ToHtml() =>
        $"<strong>{string.Concat(Children.Select(child => child.ToHtml()))}</strong>";
}