namespace Markdown;

public interface IRenderer
{
    public string RenderHtmlTags(IEnumerable<MarkdownTagInfo> renderTags, string markdownText);
}