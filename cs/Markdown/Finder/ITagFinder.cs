using Markdown.Tags;

namespace Markdown.Finder;

public interface ITagFinder
{
    public IEnumerable<Tag> FindTags(string line);
}