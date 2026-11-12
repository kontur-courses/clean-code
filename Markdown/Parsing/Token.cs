namespace Markdown.Parsing;

internal enum TokenKind
{
    Text,
    UnderscoreRun,
    LineBreak,
    ListMarker, // -
    HeadingMarker // #
}

internal sealed record Token(TokenKind Kind, string Value)
{
    public static Token LineBreak { get; } = new(TokenKind.LineBreak, "\n");
}
