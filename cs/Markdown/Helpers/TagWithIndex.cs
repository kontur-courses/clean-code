using Markdown.Tags;

namespace Markdown.Helpers;

public class TagWithIndex
{
    public ITag? Tag;
    public int Index;
    public bool IsStartedTag;

    public TagWithIndex(ITag? tag, int index, bool isStartedTag)
    {
        Tag = tag;
        Index = index;
        IsStartedTag = isStartedTag;
    }
}