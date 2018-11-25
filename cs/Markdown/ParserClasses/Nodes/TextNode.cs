using System.Collections.Generic;

namespace Markdown.ParserClasses.Nodes
{
    public class TextNode
    {
        public List<WordNode> Words { get; }

        public TextType Type { get; }

        public void Add(WordNode word) => Words.Add(word);

        public void AddRange(List<WordNode> words) => Words.AddRange(words);

        public TextNode(TextType type = TextType.Text, List<WordNode> words = null)
        {
            Type = type;
            Words = words ?? new List<WordNode>();
        }
    }
}