namespace Markdown.Lexer;

/// <summary>
/// Enum, представляющий типы токенов inline-разметки
/// </summary>
public enum InlineTokenKind
{
    /// <summary>Текст без разметки</summary>
    Text,
    /// <summary>Маркер для выделения текста курсивом</summary>
    EmphasisMarker,
    /// <summary>Маркер для выделения текста жирным</summary>
    StrongMarker,
    /// <summary>Маркер для выделения текста жирным курсивом</summary>
    StrongEmphasisMarker,
}

public sealed record InlineToken(
    InlineTokenKind Kind,
    string Value);
