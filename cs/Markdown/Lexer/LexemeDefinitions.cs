using System.Collections.Generic;

namespace Markdown.Lexer
{
    internal static class LexemeDefinitions
    {
        internal static readonly Lexeme Bold =
            new Lexeme("__", TokenType.OpeningTag | TokenType.ClosingTag);

        internal static readonly Lexeme Italic =
            new Lexeme("_", TokenType.OpeningTag | TokenType.ClosingTag);

        internal static readonly IEnumerable<Lexeme> Lexemes = new[]
        {
            Bold, Italic
        };
    }
}