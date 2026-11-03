namespace Markdown;

// Собирает HTML для текста внутри блока: использует другие парсеры,
// чтобы распознать разметку и экранирование.
public class InlineParser(
    DelimiterParser delimiterParser,
    EscapeParser escapeParser,
    HtmlRenderer htmlRenderer)
{
    private readonly DelimiterParser delimiterParser = delimiterParser;
    private readonly EscapeParser escapeParser = escapeParser;
    private readonly HtmlRenderer htmlRenderer = htmlRenderer;

    // Преобразует текст с разметкой в HTML
    public string Parse(string text)
    {
        throw new NotImplementedException();
    }
}
