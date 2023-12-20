using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class HeaderContext : ResetContext
{
    public HeaderContext(int startIndex, TagContext parentContext, TagFactory tagFactory) : base(startIndex, parentContext, tagFactory)
    {
    }

    protected override void HandleSymbolItself(char symbol)
    {
    }

    public override void CloseSingleTags(int closeIndex)
    {
        parent?.CloseSingleTags(closeIndex);
        
        if (Closed)
            return;
        
        CloseIndex = closeIndex;
        Closed = true;
    }
}