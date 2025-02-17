namespace Markdown;

public record PairToken(Token Start, Token End)
{
    // это можно упросить, но как?
    public bool IntersectWith(PairToken token) =>
        End.StartIndex > token.Start.StartIndex &&
        End.StartIndex < token.End.StartIndex &&
        Start.StartIndex < token.Start.StartIndex ||
        token.End.StartIndex > Start.StartIndex &&
        token.End.StartIndex < End.StartIndex &&
        token.Start.StartIndex < Start.StartIndex;

    public bool Contain(PairToken token) =>
        Start.StartIndex > token.Start.StartIndex &&
        End.StartIndex < token.End.StartIndex;
}