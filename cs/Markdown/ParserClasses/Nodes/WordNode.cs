namespace Markdown.ParserClasses.Nodes
{
    public class WordNode
    {
        public WordType Type { get; }
        public string Value { get; }

        public WordNode(WordType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}