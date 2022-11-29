namespace Markdown.Reading;

public class Token
{
    public char Symbol { get; }
    public int Position { get; }

    public bool IsNull { get; }

    public Token(char symbol, int position, bool isNull = false)
    {
        Symbol = symbol;
        Position = position;
        IsNull = isNull;
    }
}