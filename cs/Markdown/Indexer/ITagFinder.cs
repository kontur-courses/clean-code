using Markdown.Tags;

namespace Markdown.Indexer;

public interface ITagFinder
{
    public IEnumerable<Tag> FindTags(string line);
}