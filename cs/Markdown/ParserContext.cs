namespace Markdown;

public class ParserContext
{
    public int Position { get; private set; }
    public Token Current => Tokens[Position];
    public Token? Next => Tokens[Position + 1].Type == TokenType.Eof ? null : Tokens[Position + 1];
    public Token? Prev => Position - 1 < 0 ? null : Tokens[Position - 1];
    public readonly List<Token> Tokens;

    public ParserContext(List<Token> tokens)
    {
        Tokens = tokens;
    }

    public void IncreasePosition()
    {
        Position++;
    }
}