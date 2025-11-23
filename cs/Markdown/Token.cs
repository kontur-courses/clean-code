namespace Markdown
{
    public class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value = "")
        {
            Type = type;
            Value = value;
        }

        public static Token Heading(string v) => new(TokenType.Heading, v);
        public static Token BoldStart() => new(TokenType.BoldStart, "__");
        public static Token BoldEnd() => new(TokenType.BoldEnd, "__");
        public static Token ItalicStart() => new(TokenType.ItalicStart, "_");
        public static Token ItalicEnd() => new(TokenType.ItalicEnd, "_");
        public static Token Text(string v) => new(TokenType.Text, v);
        public static Token NewLine() => new(TokenType.NewLine, "\n");
        public static Token ListStart() => new(TokenType.ListStart, "-"); 
        public static Token ListItem() => new(TokenType.ListItem, "-"); 
        public static Token ListEnd() => new(TokenType.ListEnd, "-");
        public static Token EndOfFile() => new(TokenType.EndOfFile);
        
        public override string ToString() => $"{Type}: {Value}";
        
    }   
    
}

