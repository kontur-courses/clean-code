namespace Markdown.TokenizerClasses
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Length;

        public static Token EOF { get; } = new Token(TokenType.EOF, "");
        public static Token Null { get; } = new Token(TokenType.Null, "\0");

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
            Length = Type == TokenType.Text ? Value.Length : 1;
        }
    }
}
