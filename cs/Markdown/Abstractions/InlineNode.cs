namespace Markdown.Abstractions
{
    public abstract class InlineNode : MarkdownNode
    {
        public List<InlineNode> Children { get; set; } = new();
    }
}
