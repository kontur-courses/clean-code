using System;

namespace Markdown.Lexer
{
    [Flags]
    public enum TokenType
    {
        Text = 0,
        OpeningTag = 1,
        ClosingTag = 2
    }
}