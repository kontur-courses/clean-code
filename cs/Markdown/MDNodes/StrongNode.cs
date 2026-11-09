using Markdown.MDNodes;


namespace Markdown.MDElements
{
    public class StrongNode : PairedInlineNode
    {
        public StrongNode(string content) : base(content)
        {
        }

        protected override string OpeningTag => "<strong>";

        protected override string ClosingTag => "</strong>";
    }
}
