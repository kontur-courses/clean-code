namespace Markdown;

public class InlineParser(
    DelimiterParser delimiterParser,
    EscapeParser escapeParser,
    HtmlRenderer htmlRenderer)
{
    private readonly DelimiterParser delimiterParser = delimiterParser;
    private readonly EscapeParser escapeParser = escapeParser;
    private readonly HtmlRenderer htmlRenderer = htmlRenderer;

    public string Parse(string text)
    {
        throw new NotImplementedException();
    }
}
