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
        {
            Position = start;
            Length = value.Length;
            Value = value;
            Type = type;
            Lexeme = lexeme;
        }
    }
}