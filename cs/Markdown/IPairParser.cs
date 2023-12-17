namespace Markdown;

public interface IPairParser
{
    public IEnumerable<TagPair> ParseTagPairs(string markdownText);
}