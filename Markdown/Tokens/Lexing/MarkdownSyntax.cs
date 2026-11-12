namespace Markdown.Tokens.Lexing;

public sealed record SymbolDefinition(string Pattern, bool LineStartOnly = false);

public static class MarkdownSyntax
{
    public const string HeaderPrefix = "# ";
    public const string ParagraphBreaker = "\n";
    public const string DoubleUnderscore = "__";
    public const string Underscore = "_";


    public static IReadOnlyList<SymbolDefinition> Symbols =
    [
        new(HeaderPrefix, true),
        new(DoubleUnderscore),
        new(Underscore),
    ];

    public static bool IsEscapable(char character) => character == '\\' || Symbols.Any(s => s.Pattern[0] == character);
}