using System;

namespace Markdown.Tokens
{
    public partial class Token
    {
        public static Func<string, Token> Text => s => new Token(TokenType.Text, s);
        public static Token Italics => new(TokenType.Italics, "_");
        public static Token Bold => new(TokenType.Bold, "__");
        public static Token Escape => new(TokenType.Escape, "\\");
        public static Token Header1 => new(TokenType.Header1, "# ");
        public static Token NewLine => new(TokenType.NewLine, "\n");
        public static Token OpenImageAlt => new(TokenType.OpenImageAlt, "![");
        public static Token CloseImageAlt => new(TokenType.CloseImageAlt, "](");
        public static Token CloseImageTag => new(TokenType.CloseBracket, ")");
    }
}