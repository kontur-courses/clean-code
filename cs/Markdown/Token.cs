namespace Markdown
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

        public static Token Cursive => new(TokenType.Cursive, "_");
        
        public static Token Bold => new(TokenType.Bold, "__");
        
        public static Token Escape => new(TokenType.Escape, "\\");
        
        public static Token Text(string text) => new(TokenType.Text, text);
        
        public static Token Header1 => new(TokenType.Header1, "# ");
        
        public static Token NewLine => new(TokenType.NewLine, "\n");
    }
}