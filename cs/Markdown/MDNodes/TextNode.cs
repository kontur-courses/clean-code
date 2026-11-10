using Markdown.Abstractions;

namespace Markdown.MDNodes
{
    public sealed class TextNode : InlineNode
    {
        public string Content { get; set; }

        public TextNode(string content)
        {
            Content = content;
        }

        public override string Render() => Content;
    }
}
