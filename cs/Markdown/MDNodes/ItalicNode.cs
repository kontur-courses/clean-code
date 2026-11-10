using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.MDElements
{
    public class ItalicNode : PairedInlineNode
    {
        public ItalicNode(List<InlineNode> children) : base(children) { }

        protected override string OpeningTag => "<em>";

        protected override string ClosingTag => "</em>";
    }
}
