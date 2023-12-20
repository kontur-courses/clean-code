namespace Markdown;

public interface IParser
{
    public IEnumerable<MarkdownTagInfo> GetTagsToRender(string markdownText);
}