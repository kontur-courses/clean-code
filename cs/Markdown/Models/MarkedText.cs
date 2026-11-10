namespace Markdown.Models;

public sealed class MarkedText
{
    public MarkedText(IReadOnlyList<Token> tokens)
    {
        Tokens = tokens;
    }

    public IReadOnlyList<Token> Tokens { get; }
}