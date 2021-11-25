using System;
using Markdown.TokenFormatter.Renders;

namespace Markdown.Tokens
{
    public partial class Token
    {
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; }
        public string Value { get; }

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