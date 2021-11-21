namespace Markdown.Tokens
{
    public readonly struct Token
    {
        public readonly TokenType Type;
        public readonly string Value;

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public static readonly Token Cursive = new(TokenType.Cursive, "_");

        public static readonly Token Bold = new(TokenType.Bold, "__");

        public static readonly Token Escape = new(TokenType.Escape, "\\");

        public static Token Text(string text) => new(TokenType.Text, text);

        public static readonly Token Header1 = new(TokenType.Header1, "# ");

        public static readonly Token NewLine = new(TokenType.NewLine, "\n");
    }
}