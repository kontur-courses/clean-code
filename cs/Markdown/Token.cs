namespace Markdown
{
    public enum TokenType
    {
        Text,
        Italics,
        Strong,
        Escape,
        NewLine,
        Header1
    }

    public class Token
    {
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }
        public string Value { get; }
    }
}