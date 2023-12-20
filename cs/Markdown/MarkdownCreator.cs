namespace Markdown;

public class MarkdownCreator
{
    private readonly IParser parser;
    private readonly IRenderer renderer;

    public MarkdownCreator(IParser parser, IRenderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    public string MarkdownText(string markdownText)
    {
        var tagsToRender = parser.GetTagsToRender(markdownText);
        var renderedText = renderer.RenderHtmlTags(tagsToRender, markdownText);
        return renderedText;
    }
}