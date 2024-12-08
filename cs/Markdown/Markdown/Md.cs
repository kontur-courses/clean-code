namespace Markdown;

public class Md
{
    private readonly IMarkdownConverter converter;
    private readonly IMarkdownParser parser;

    public Md(IMarkdownConverter converter, IMarkdownParser parser)
    {
        this.converter = converter;
        this.parser = parser;
    }

    public string Render(string markdown)
    {
        var tokens = parser.ParseTextToTokens(markdown);
        return converter.Convert(tokens);
    }
}
