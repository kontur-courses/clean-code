namespace Markdown;

// Класс для поиска пар маркеров выделения в тексте
public class DelimiterParser (
    EscapeParser escapeParser)
{
    private readonly EscapeParser escapeParser = escapeParser;

    // Метод для поиска пар маркеров выделения в тексте, возвращает список объектов DelimiterPair, представляющих найденные пары маркеров
    public IReadOnlyList<DelimiterPair> FindPairs(string text)
    {
        throw new NotImplementedException();
    }

    // Проверяет, подходит ли маркер на роль открывающего
    private bool CanOpen(string text, int delimiterStartIndex, int delimiterLength)
    {
        throw new NotImplementedException();
    }

    // Проверяет, подходит ли маркер на роль закрывающего
    private bool CanClose(string text, int delimiterStartIndex, int delimiterLength)
    {
        throw new NotImplementedException();
    }
}
