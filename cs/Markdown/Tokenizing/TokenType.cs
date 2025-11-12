namespace Markdown.Tokenizing;

public enum TokenType
{
    Text,
    Underscore,
    DoubleUnderscore,
    Escape,
    Hash,
    Whitespace,
    EndOfLine,
    EndOfFile,
    LeftBracket,
    RightBracket,
    LeftParen,
    RightParen
}