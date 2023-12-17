namespace Markdown;

public interface IRenderer
{
    public string RenderTags(IEnumerable<TagPair> tagPairs, string markdownText);
}