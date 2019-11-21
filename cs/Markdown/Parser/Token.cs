namespace Markdown.Parser
{
    public class Token
    {
        public Token(TokenType type)
        {
            Type = type;
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TokenType Type { get; }
        public string Value { get; }
    }
}