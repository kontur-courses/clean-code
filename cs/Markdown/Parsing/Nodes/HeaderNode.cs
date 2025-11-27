namespace Markdown.Parsing.Nodes;

public class HeaderNode : MarkdownNode
{
    public int Level { get; set; }
    public List<MarkdownNode> Children { get; set; } = [];

    public override string ToHtml() =>
        $"<h{Level}>{string.Concat(Children.Select(child => child.ToHtml()))}</h{Level}>";
}