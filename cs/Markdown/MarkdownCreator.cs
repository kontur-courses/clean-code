namespace Markdown;

public class MarkdownCreator
{
    private readonly Parser parser;
    private readonly Renderer renderer;

    public MarkdownCreator(Parser parser, Renderer renderer)
    {
        this.parser = parser;
        this.renderer = renderer;
    }

    public string MarkdownText(string markdownText)
    {
        var tagsToRender = parser.GetTagsToRender(markdownText);
        var renderedText = renderer.RenderTags(tagsToRender, markdownText);
        return renderedText;
    }
}