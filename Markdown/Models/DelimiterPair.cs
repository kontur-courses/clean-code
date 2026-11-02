namespace Markdown;

public class DelimiterPair
{
    public int OpeningStartIndex { get; }
    public int ClosingStartIndex { get; }
    public DelimiterType Type { get; }

// Начала открывающего и закрывающего маркера, тип нужен для определения его длины (Пока Emphasis и Strong, "_" и "__" соответственно)
    public DelimiterPair(
        int openingStartIndex,
        int closingStartIndex,
        DelimiterType type)
    {
        OpeningStartIndex = openingStartIndex;
        ClosingStartIndex = closingStartIndex;
        Type = type;
    }
}
