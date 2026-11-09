namespace Markdown.Parser;

public record LinkMatch(
    int OpenBracketIndex,
    int CloseBracketIndex,
    int CloseParenIndex,
    string Url
);