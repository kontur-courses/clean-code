using Markdown.MDNodes;

namespace Markdown.MDElements
{
    public class ItalicNode : PairedInlineNode
    {
        protected override string OpeningTag => "<em>";

        protected override string ClosingTag => "</em>";
    }
}
