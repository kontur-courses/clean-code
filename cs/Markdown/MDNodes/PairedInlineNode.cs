using Markdown.Abstractions;


namespace Markdown.MDNodes
{
    public abstract class PairedInlineNode : InlineNode
    {
        protected abstract string OpeningTag { get; }
        protected abstract string ClosingTag { get; }

        protected PairedInlineNode(List<InlineNode> children)
        {
            Children = children ?? new List<InlineNode>();
        }

        public override string Render()
        {
            var inner = string.Join("", Children.Select(c => c.Render()));
            return $"{OpeningTag}{inner}{ClosingTag}";
        }
    }
}
