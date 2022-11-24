using Markdown.Tags;

namespace Markdown.Helpers;

public class TagWithIndex
{
    public ITag? Tag;
    public int Index;

    public TagWithIndex(ITag? tag, int index)
    {
        Tag = tag;
        Index = index;
    }
}