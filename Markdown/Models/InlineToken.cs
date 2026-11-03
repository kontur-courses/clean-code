namespace Markdown.Models;

// представление отдельных элементов текста
internal enum InlineTokenType
{
    Text,
    Emphasis,
    Strong
}

internal sealed record InlineToken(
    InlineTokenType Type,
    string Value,
    int SourceIndex
);