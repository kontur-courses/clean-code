namespace Markdown.Core
{
    public class TokenPart
    {
        public string Value { get; }
        public bool Escaped { get; set; }

        public TokenPart(string value, bool escaped)
        {
            Value = value;
            Escaped = escaped;
        }

        public TokenPart(string value) : this(value, false) { }
        public TokenPart(bool escaped) : this(null, escaped) { }
    }
}
