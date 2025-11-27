namespace Markdown.Parsing.Nodes;

public class ItalicNode : MarkdownNode
{
    public List<MarkdownNode> Children { get; set; } = [];
    public override string ToHtml() => $"<em>{string.Concat(Children.Select(child => child.ToHtml()))}</em>";
}