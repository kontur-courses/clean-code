namespace Markdown
{
    public class Token
    {
        public string Separator { get; }
        public int Position { get; }
        public string Value { get; }
        public string ParentSeparator { get; }

        public Token(string separator, int position, string value, string parentSeparator)
        {
            Separator = separator;
            Position = position;
            Value = value;
            ParentSeparator = parentSeparator;
        }

        public Token(string separator, int position, string value) : this(separator, position, value, null) { }
    }
}