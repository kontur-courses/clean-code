namespace Markdown.Tokens;

public enum TokenKind
{
    Text,

    Underscore,

    DoubleUnderscore,

    LeftBracket,

    RightBracket,

    /// <summary>
    ///     Скобки со ссылкой. Token.Value - url вместе со скобками.
    /// </summary>
    LinkUrl
}