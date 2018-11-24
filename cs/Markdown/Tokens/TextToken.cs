namespace Markdown.Tokens
{
    public class TextToken : IToken
    {
        public string Value { get; }

        public TextToken(string value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TextToken objAsToken)) return false;
            return objAsToken.Value.Equals(this.Value);
        }
    }
}