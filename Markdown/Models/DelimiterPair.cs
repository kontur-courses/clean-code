namespace Markdown;

// Нужен для хранения одного выделения
public class DelimiterPair
{
// Индекс начала открывающего маркера в тексте
    public int OpeningStartIndex { get; } 
// Индекс начала закрывающего маркера в тексте
    public int ClosingStartIndex { get; }
// Тип маркера (щас есть Emphasis и Strong)
    public DelimiterType Type { get; }


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
