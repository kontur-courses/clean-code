namespace Markdown.Models;

public sealed class MarkedText
{
    public IReadOnlyList<Token> Tokens { get; }

    public MarkedText(IReadOnlyList<Token> tokens)
    {
        Tokens = tokens;
    }
}