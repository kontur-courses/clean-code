using System.Collections.Generic;

namespace Markdown.ParserClasses.Nodes
{
    public class SentenceNode
    {
        public List<TextNode> Texts { get; } = new List<TextNode>();

        public void Add(TextNode textNode) => Texts.Add(textNode);
    }
}