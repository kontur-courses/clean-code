namespace Markdown.Core.Parsing.Nodes;

public class LinkNode(string href, IList<InlineNode> inlines) : InlineNode
{
    public string Href { get; } = href;
    public IList<InlineNode> Inlines { get; } = inlines;
}
