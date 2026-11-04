using Markdown.Rendering;

namespace Markdown.Parsing;

/// <summary>
/// Распознаёт блочную разметку и передаёт содержимое блоков в InlineParser
/// Скорее всего деление на блоки будет по пустым строкам
/// </summary>
public class BlockParser()
{
    private readonly InlineParser inlineParser = new();
    private readonly HtmlRenderer htmlRenderer = new();

    public string Parse(string text)
    {
        throw new NotImplementedException();
    }
}
