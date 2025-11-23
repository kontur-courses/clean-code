namespace Markdown;

public sealed class TokenCursor(List<Token> tokens)
{
    public List<Token> tokens {get; } = tokens ?? throw new ArgumentNullException(nameof(tokens));
    public int Position { get; private set; } = 0;

    public readonly int TokenCount = tokens.Count;
    public bool End => Position >= tokens.Count - 1;
    public Token? Current => End ? null : tokens[Position];

    public void Move(int offset = 1)
    {
        if (Position + offset < tokens.Count) 
            Position += offset;
        else
            throw new IndexOutOfRangeException();
    }

    public Token? TakeNext() => Position + 1 < TokenCount ? tokens[Position + 1] : null;
}