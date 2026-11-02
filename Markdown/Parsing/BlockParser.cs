namespace Markdown;

public class BlockParser(
    InlineParser inlineParser,
    HtmlRenderer htmlRenderer)
{
    private readonly InlineParser inlineParser = inlineParser;
    private readonly HtmlRenderer htmlRenderer = htmlRenderer;

    public string Parse(string text)
    {
        throw new NotImplementedException();
    }
}
