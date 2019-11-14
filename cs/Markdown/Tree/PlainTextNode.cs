namespace Markdown.Tree
{
    public class PlainTextNode : Node
    {
        public PlainTextNode(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override string GetText()
        {
            return Value;
        }
    }
}