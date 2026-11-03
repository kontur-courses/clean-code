namespace Markdown;

//  Распознаёт блочную разметку и передаёт содержимое блоков в InlineParser
public class BlockParser(
    InlineParser inlineParser,
    HtmlRenderer htmlRenderer)
{
    private readonly InlineParser inlineParser = inlineParser;
    private readonly HtmlRenderer htmlRenderer = htmlRenderer;

    // Находит блоки в тексте и преобразует их в HTML
    public string Parse(string text)
    {
        throw new NotImplementedException();
    }
}
