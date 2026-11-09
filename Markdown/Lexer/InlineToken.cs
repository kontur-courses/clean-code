namespace Markdown.Lexer;

/// <summary>
/// Enum, представляющий типы токенов inline-разметки
/// </summary>
public enum InlineTokenKind
{
    Text, // Текст без разметки
    EmphasisMarker, // Маркер для выделения текста курсивом
    StrongMarker, // Маркер для выделения текста жирным
    StrongEmphasisMarker, // Маркер для выделения текста жирным курсивом
}

public sealed record InlineToken(
    InlineTokenKind Kind,
    string Value);
