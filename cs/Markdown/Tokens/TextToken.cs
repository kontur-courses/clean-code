namespace Markdown.Tokens
{
    public class TextToken : IToken
    {
        public TokenType Type { get; } = TokenType.Text;
        public string Value { get; }

        public TextToken(string value)
        {
            Value = value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TextToken objAsToken)) return false;
            return objAsToken.Type == this.Type && objAsToken.Value.Equals(this.Value);
        }
    }
}