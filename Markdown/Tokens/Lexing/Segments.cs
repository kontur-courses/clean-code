namespace Markdown.Tokens.Lexing;

public abstract record RawSegment;

public sealed record TextSegment(string Value) : RawSegment;

public sealed record SymbolSegment(string Symbol) : RawSegment;

public sealed record LineBreakSegment : RawSegment;
