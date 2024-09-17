using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class HeaderContext : ResetContext
{
    public HeaderContext(int startIndex, TagContext parentContext, TagFactory tagFactory, bool isScreened) 
        : base(startIndex, parentContext, tagFactory, isScreened)
    {
    }
}