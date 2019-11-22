namespace Markdown.Lexer
{
    internal class Token
    {
        internal readonly int Position;
        internal readonly int Length;
        internal readonly string Value;
        internal readonly TokenType Type;
        internal readonly Lexeme Lexeme;

        internal Token(int start, string value, TokenType type, Lexeme lexeme = null)
            : this(start, value.Length, value, type, lexeme)
        {
        }

        internal Token(int start, int length, string value, TokenType type, Lexeme lexeme = null)
        {
            Position = start;
            Length = length;
            Value = value;
            Type = type;
            Lexeme = lexeme;
        }

        public override string ToString() => $"[{Type}] '{Value}'";
    }
}