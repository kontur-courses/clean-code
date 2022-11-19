namespace Markdown.Indexer;

public interface ITagIndexer
{
    public IEnumerable<TagPosition> IndexTags(string line);
}