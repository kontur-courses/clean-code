namespace Markdown;

public class Token
{
    public char Symbol { get; }
    public int Position { get; }
    public bool IsStartOfWord { get; }

    public Token(char symbol, int position, bool isStartOfWord)
    {
        Symbol = symbol;
        Position = position;
        IsStartOfWord = isStartOfWord;
    }

}