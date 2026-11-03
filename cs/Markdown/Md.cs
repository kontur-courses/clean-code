namespace Markdown;

// Управляет преобразованием всего документа
public class Md
{
    private readonly BlockParser blockParser;
    private readonly InlineParser inlineParser;

    public Md()
    {
        throw new NotImplementedException();
    }

    public static string Render(string markdownText)
    {
        throw new NotImplementedException();
    }
}

// Разделяет текст на абзацы и распознаёт заголовки
public class BlockParser
{
    public IReadOnlyList<string> SplitIntoParagraphs(string text)
    {
        throw new NotImplementedException();
    }

    public bool IsHeading(string paragraph)
    {
        throw new NotImplementedException();
    }
}

// Преобразует внутристрочную разметку в HTML
public class InlineParser
{
    public string Render(string text)
    {
        throw new NotImplementedException();
    }
}