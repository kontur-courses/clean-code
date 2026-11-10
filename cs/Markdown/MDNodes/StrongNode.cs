using Markdown.Abstractions;
using Markdown.MDNodes;


namespace Markdown.MDElements
{
    public class StrongNode : PairedInlineNode
    {


        public StrongNode(List<InlineNode> children) : base(children) { }

        protected override string OpeningTag => "<strong>";

        protected override string ClosingTag => "</strong>";
    }
}
