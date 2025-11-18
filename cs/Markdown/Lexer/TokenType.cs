namespace Markdown;

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
    OpenBracket,
    CloseBracket,
    OpenParen,
    CloseParen,
    AutoLinkOpen,
    AutoLinkClose
}