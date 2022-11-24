namespace Markdown;

public class Token
{
    public char Symbol { get; }
    public int Position { get; }

    public Token(char symbol, int position)
    {
        Symbol = symbol;
        Position = position;
    }
}