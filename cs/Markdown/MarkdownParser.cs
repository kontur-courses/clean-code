using Markdown.Nodes;

namespace Markdown;

public class MarkdownParser(InlineTokenizer tokenizer)
{
    private readonly InlineTokenizer _tokenizer = tokenizer;

    public IReadOnlyList<BlockNode> Parse(string markdown)
    {
        //сплитуем markdown на строки
        //для каждой вызывает ParseBlock
        //Получаем распарсенный BlockNode
        //Добавляем его в список

        throw new NotImplementedException();
    }

    private BlockNode ParseBlock(string line)
    {
        //Определяем тип блока (заголовок или параграф)
        //Передаем строку (с учетом сдвига "# ") в ParseInline
        //Получаем список InlineNode и создаем соответствующий BlockNode

        throw new NotImplementedException();
    }

    private IReadOnlyList<InlineNode> ParseInline(string text, int startIndex = 0)
    {
        //разбиваем текст на токены c помощью InlineTokenizer
        //вызываем FindTokenPairs для поиска пар токенов
        //вызываем BuildNodes для построения InlineNode по найденным парам
        throw new NotImplementedException();
    }

    private IReadOnlyList<TokenPair> FindTokenPairs(
        IReadOnlyList<Token> tokens,
        string text)
    {
        // Сопоставляем маркеры выделения
        // Проверяем ограничения, вложенность и пересечения
        // text нужен для правил, зависящих от содержимого
        throw new NotImplementedException();
    }

    private IReadOnlyList<InlineNode> BuildNodes(
        string text,
        int startIndex,
        IReadOnlyList<Token> tokens,
        IReadOnlyList<TokenPair> pairs)
    {
        // Строим дерево по подтверждённым парам.
        // Обычный текст сохраняем в TextNode.
        // По Escape убираем экранирующие слеши из текста узлов.
        throw new NotImplementedException();
    }
}