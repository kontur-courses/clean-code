using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts.Abstracts;

public abstract class ResetContext : TagContext
{
    protected ResetContext(int startIndex, TagContext? parent, TagFactory tagFactory, bool isScreened) 
        : base(startIndex, parent, tagFactory, isScreened)
    {
    }

    protected override void HandleSymbolItself(char symbol)
    {
    }
    
    public TagContext SwitchToOpenContext()
    {
        var nowParent = Parent;
        
        while (nowParent is not null)
        {
            if (!nowParent.Closed)
                return nowParent;

            nowParent = nowParent.Parent;
        }

        return this;
    }
    
    public override void CloseSingleTags(int closeIndex)
    {
        Parent?.CloseSingleTags(closeIndex);
        
        if (Closed)
            return;
        
        CloseIndex = closeIndex;
        Closed = true;
    }
}