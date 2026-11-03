namespace Markdown;

public class Md
{
    private readonly BlockParser blockParser;

    // BlockParser разбирает блоки, InlineParser — разметку внутри них;
    // DelimiterParser и EscapeParser распознают маркеры, HtmlRenderer формирует HTML.
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
