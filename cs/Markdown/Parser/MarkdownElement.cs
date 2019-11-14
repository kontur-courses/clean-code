namespace Markdown.Parser
{
    internal abstract class MarkdownElement : Element
    {
        internal MarkdownElement(INode parent, string value) : base(parent, value)
        {
        }
    }
}