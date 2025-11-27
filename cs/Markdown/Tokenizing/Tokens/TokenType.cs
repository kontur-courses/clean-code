namespace Markdown.Tokenizing.Tokens;

/// <summary>
/// Enum описывающий поддерживаемые типы токенов
/// </summary>
public enum TokenType
{
    Text,
    EscapeCharacter,
    WhiteSpace,
    Underscore,
    Hashtag,
    LeftSquareBracket,
    RightSquareBracket,
    LeftBracket,
    RightBracket
}