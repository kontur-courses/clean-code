using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public sealed class TextNode : InlineNode
    {
        public TextNode(string content) : base(content)
        {
        }

        public override string Render() => Content;
    }
}
