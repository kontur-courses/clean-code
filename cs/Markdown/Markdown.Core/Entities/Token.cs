namespace Markdown.Core.Entities
{
    public class Token
    {
        public string Value { get; }
        public TokenType TokenType { get; }

        public Token(string value, TokenType tokenType )
        {
            Value = value;
            TokenType = tokenType;
        }
    }
}