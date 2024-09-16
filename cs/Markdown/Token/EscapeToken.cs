﻿namespace Markdown.Token;

public class EscapeToken : IToken
{
    private const string TokenSeparator = "\\";
    private const bool HasPair = false;

    public int Length => TokenSeparator.Length;
    public string Separator => TokenSeparator;
    public bool IsPair => HasPair;
    public bool IsClosed { get; set; }
    public int Position { get; }
    public bool IsParametrized => false;
    public List<string> Parameters { get; set; }
    public int TokenSymbolsShift { get; set; }

    public EscapeToken(int position)
    {
        Position = position;
    }

    public bool IsValid(string source, List<IToken> tokens, IToken currentToken)
    {
        return true;
    }
}