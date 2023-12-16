using System.Collections.Immutable;
using System.Text;
using MarkDown.Enums;
using MarkDown.Tags;

namespace MarkDown.TagContexts;

public abstract class TagContext
{
    protected List<TagContext> InnerContexts = new();
    protected readonly TagContext? parent;

    protected TagContext(int startIndex, TagContext? parent, Tag creator)
    {
        this.parent = parent;
        StartIndex = startIndex;
        Creator = creator;
    }

    public int StartIndex { get; init; }
    protected Tag Creator { get; }

    public int CloseIndex { get; protected set; }
    
    public bool Closed { get; protected set; }
    
    public bool CanClose { get; protected set; } = true;

    public bool TryClose(TagName tagCloseName, int closeIndex)
    {
        if (parent != null && parent.TryClose(tagCloseName, closeIndex))
            return true;
        
        if (Closed)
            return false;
        
        Closed = Creator.TagName == tagCloseName && CanClose;
        CloseIndex = closeIndex;
        
        return Closed;
    }

    public void AddInnerContext(TagContext tagContext)
    {
        if (!Closed)
            InnerContexts.Add(tagContext);
        
        parent?.AddInnerContext(tagContext);
    }

    public void HandleSymbol(char symbol)
    {
        HandleSymbolItself(symbol);
        parent?.HandleSymbol(symbol);
    }

    protected abstract void HandleSymbolItself(char symbol);

    public abstract (int start, int end) ConvertToHtml(string text, StringBuilder sb, MarkDownEnvironment environment);

    public abstract void CloseSingleTags(int closeIndex);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int) Creator.TagName * 397) ^ StartIndex;
        }
    }
}