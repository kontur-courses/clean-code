namespace Markdown.Parsing.Nodes;

public class ParagraphNode : MarkdownNode
{
    public List<MarkdownNode> Children { get; set; } = [];

    public override string ToHtml() => $"<p>{string.Concat(Children.Select(child => child.ToHtml()))}</p>";
}