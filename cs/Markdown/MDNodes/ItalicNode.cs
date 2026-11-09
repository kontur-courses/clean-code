using Markdown.MDNodes;

namespace Markdown.MDElements
{
    public class ItalicNode : PairedInlineNode
    {
        public ItalicNode(string content) : base(content)
        {
        }

        protected override string OpeningTag => "<em>";

        protected override string ClosingTag => "</em>";
    }
}
