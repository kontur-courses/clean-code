namespace Markdown.Core
{
    public class TokenPart
    {
        public string Value { get; private set; }
        public bool NoNeedToParse { get; private set; }

        public TokenPart(string value, bool escaped)
        {
            Value = value;
            NoNeedToParse = escaped;
        }

        public TokenPart(string value) : this(value, false) { }
        public TokenPart(bool escaped) : this(null, escaped) { }
    }
}
