namespace Markdown.Tokens.Lexing;

public interface ISymbolInterpreter
{
    bool CanHandle(SymbolSegment segment);

    Token Interpret(IReadOnlyList<RawSegment> segments, int index);
}

public sealed class UnderscoreInterpreter : ISymbolInterpreter
{
    public bool CanHandle(SymbolSegment segment)
        => segment.Symbol is MarkdownSyntax.Underscore or MarkdownSyntax.DoubleUnderscore;

    public Token Interpret(IReadOnlyList<RawSegment> segments, int index)
    {
        var symbol = (SymbolSegment)segments[index];
        var (canOpen, canClose) = GetFlags(segments, index);

        if (!canOpen && !canClose)
            return new TextToken(symbol.Symbol);

        return symbol.Symbol == MarkdownSyntax.DoubleUnderscore
            ? new DoubleUnderscoreToken(canOpen, canClose)
            : new UnderscoreToken(canOpen, canClose);
    }

    private static (bool CanOpen, bool CanClose) GetFlags(IReadOnlyList<RawSegment> segments, int index)
    {
        var before = SegmentNeighbors.CharBefore(segments, index);
        var after = SegmentNeighbors.CharAfter(segments, index);
        var nearDigits = char.IsDigit(before) || char.IsDigit(after);

        return (CanOpen: !char.IsWhiteSpace(after) && !nearDigits,
            CanClose: !char.IsWhiteSpace(before) && !nearDigits);
    }
}

public sealed class HeaderInterpreter : ISymbolInterpreter
{
    public bool CanHandle(SymbolSegment segment) => segment.Symbol == MarkdownSyntax.HeaderPrefix;

    public Token Interpret(IReadOnlyList<RawSegment> segments, int index) => new HeaderMarkerToken();
}


public static class SegmentNeighbors
{
    private const char WhiteSpace = ' ';

    public static char CharBefore(IReadOnlyList<RawSegment> segments, int index)
        => index > 0 ? LastChar(segments[index - 1]) : WhiteSpace;

    public static char CharAfter(IReadOnlyList<RawSegment> segments, int index)
        => index + 1 < segments.Count ? FirstChar(segments[index + 1]) : WhiteSpace;

    private static char FirstChar(RawSegment segment) => segment switch
    {
        TextSegment { Value.Length: > 0 } text => text.Value[0],
        SymbolSegment symbol => symbol.Symbol[0],
        _ => WhiteSpace
    };

    private static char LastChar(RawSegment segment) => segment switch
    {
        TextSegment { Value.Length: > 0 } text => text.Value[^1],
        SymbolSegment symbol => symbol.Symbol[^1],
        _ => WhiteSpace
    };
}