namespace Markdown;

public class Markdown
{
    private readonly IPairParser parser;
    private readonly IRenderer renderer;

    public Markdown(IPairParser parser, IRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    public string MarkdownText(string markdownText)
    {
        var tagPairs = parser.ParseTagPairs(markdownText);
        var renderedText = renderer.RenderTags(tagPairs, markdownText);
        return renderedText;
    }
}