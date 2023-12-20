using MarkDown.Tags;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts.Abstracts;

public abstract class ResetContext : TagContext
{
    protected ResetContext(int startIndex, TagContext? parent, TagFactory tagFactory) : base(startIndex, parent, tagFactory)
    {
    }
    
    public TagContext SwitchToOpenContext()
    {
        var nowParent = parent;
        
        while (nowParent is not null)
        {
            if (!nowParent.Closed)
                return nowParent;

            nowParent = nowParent.parent;
        }

        return this;
    }
}