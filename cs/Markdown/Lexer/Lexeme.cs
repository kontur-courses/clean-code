namespace Markdown.Lexer
{
    internal class Lexeme
    {
        internal readonly string Representation;
        internal readonly TokenType PossibleType;

        internal Lexeme(string representation, TokenType possibleType)
        {
            Representation = representation;
            PossibleType = possibleType;
        }
    }
}