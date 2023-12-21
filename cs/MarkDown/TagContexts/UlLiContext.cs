using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class UlLiContext : ResetContext
{
    public UlLiContext(int startIndex, TagContext? parent, TagFactory tagFactory, bool isScreened) 
        : base(startIndex, parent, tagFactory, isScreened)
    {
    }
}