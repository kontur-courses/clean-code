using Markdown.MDNodes;


namespace Markdown.MDElements
{
    public class StrongNode : PairedInlineNode
    {
        protected override string OpeningTag => "<strong>";

        protected override string ClosingTag => "</strong>";
    }
}
