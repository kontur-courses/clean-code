using System;
using Markdown.TokenFormatter;
using Markdown.TokenFormatter.Renders;

namespace Markdown.Tokens
{
    public class Token
    {
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }
        public string Value { get; }

        public static Func<string, Token> Text => s => new Token(TokenType.Text, s);
        public static Token Italics => new(TokenType.Italics, "_");
        public static Token Bold => new(TokenType.Bold, "__");
        public static Token Escape => new(TokenType.Escape, "\\");
        public static Token Header1 => new(TokenType.Header1, "# ");
        public static Token NewLine => new(TokenType.NewLine, "\n");
        public static Token OpenImageAlt => new(TokenType.OpenImageAlt, "![");
        public static Token CloseImageAlt => new(TokenType.CloseImageAlt, "](");
        public static Token CloseImageTag => new(TokenType.CloseBracket, ")");
        public virtual string Render(IRenderer renderer) => renderer.RenderText(Value);

        private bool Equals(Token other) => TokenType == other.TokenType && Value == other.Value;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode() => HashCode.Combine((int) TokenType, Value);
    }
}