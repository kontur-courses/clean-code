using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public class ItalicNode : PairedInlineNode
    {
        public ItalicNode(List<InlineNode> children) : base(children) { }

        protected override string OpeningTag => "<em>";

        protected override string ClosingTag => "</em>";
    }
}
