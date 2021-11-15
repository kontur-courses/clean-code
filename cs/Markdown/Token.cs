namespace Markdown
{
    public class Token
    {
        public string Value { get; }
        public TokenType Type { get; }

        public Token(string value, TokenType tokenType)
        {
            Value = value;
            Type = tokenType;
        }
    }
}
