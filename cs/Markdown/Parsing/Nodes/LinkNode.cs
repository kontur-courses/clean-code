namespace Markdown.Parsing.Nodes;

public class LinkNode : MarkdownNode
{
    public string Url { get; set; }
    public List<MarkdownNode> Children { get; set; } = [];

    public override string ToHtml() =>
        $"<a href=\"{Url}\">{string.Concat(Children.Select(c => c.ToHtml()))}</a>";
}