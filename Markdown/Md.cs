namespace Markdown;

public class Md
{
    private readonly BlockParser blockParser;

    public Md()
    {
        var htmlRenderer = new HtmlRenderer();
        var escapeParser = new EscapeParser();
        var delimiterParser = new DelimiterParser(escapeParser);
        var inlineParser = new InlineParser(delimiterParser, escapeParser, htmlRenderer);
        blockParser = new BlockParser(inlineParser, htmlRenderer);
    }

    public string Render(string markdownText)
    {
        throw new NotImplementedException();
    }
}
