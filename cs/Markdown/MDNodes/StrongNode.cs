using Markdown.Abstractions;


namespace Markdown.MDNodes
{
    public class StrongNode : PairedInlineNode
    {


        public StrongNode(List<InlineNode> children) : base(children) { }

        protected override string OpeningTag => "<strong>";

        protected override string ClosingTag => "</strong>";
    }
}
