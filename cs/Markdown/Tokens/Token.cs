using System;

namespace Markdown.Tokens
{
    public readonly struct Token
    {
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public static Token Italics => new(TokenType.Italics, "_");
        public static Token Bold => new(TokenType.Bold, "__");
        public static Token Escape => new(TokenType.Escape, "\\");
        public static Token Header1 => new(TokenType.Header1, "# ");
        public static Token NewLine => new(TokenType.NewLine, "\n");
        public static Func<string, Token> Text => s => new Token(TokenType.Text, s);

        public TokenType TokenType { get; }
        public string Value { get; }
    }
}