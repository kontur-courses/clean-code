using System.Text;
using MarkDown.Enums;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts.Abstracts;

public abstract class TagContext
{
    protected List<TagContext> InnerContexts = new();
    protected internal readonly TagContext? parent;

    protected TagContext(int startIndex, TagContext? parent, TagFactory tagFactory)
    {
        this.parent = parent;
        StartIndex = startIndex;
        TagFactory = tagFactory;
    }

    public bool Closed { get; protected set; }
    protected int CloseIndex { get; set; }
    protected bool ConsiderInCreatingHtml { get; set; } = true;
    private int StartIndex { get; init; }
    private TagFactory TagFactory { get; }
    private bool isIntersected { get; set; }
    
    public bool TryClose(TagName tagCloseName, int closeIndex, out TagContext closed)
    {
        closed = this;
        
        if (parent != null && parent.TryClose(tagCloseName, closeIndex, out closed))
            return true;
        
        if (Closed)
            return false;
        
        Closed = TagFactory.TagName == tagCloseName && ConsiderInCreatingHtml;
        CloseIndex = closeIndex;
        
        return Closed;
    }

    public void AddInnerContext(TagContext tagContext)
    {
        if (!Closed)
        {
            InnerContexts.Add(tagContext);
            return;
        }
        
        parent?.AddInnerContext(tagContext);
    }

    public void HandleSymbol(char symbol)
    {
        HandleSymbolItself(symbol);
        parent?.HandleSymbol(symbol);
    }

    private bool HasUnsupportedParents(MarkDownEnvironment environment)
    {
        if (!Closed)
            return false;
        
        var nowParent = parent;
        var unsupported = environment.GetUnsupportedParentsFor(TagFactory);

        while (nowParent is not null)
        {
            if (nowParent is HighlightContext
                && nowParent.Closed
                && nowParent.CloseIndex < CloseIndex)
                return true;
            
            if (nowParent.Closed 
                && nowParent.CloseIndex > StartIndex 
                && unsupported.Any(e => e.Equals(nowParent.TagFactory.TagName)))
                return true;
            
            nowParent = nowParent.parent;
        }
        
        return false;
    }

    protected internal void MarkIntersectedTags(List<TagContext> previousClosed)
    {
        if (Closed && this is HighlightContext)
        {
            for (var i = previousClosed.Count - 1; i >= 0; i--)
                if (previousClosed[i].CloseIndex > StartIndex && CloseIndex > previousClosed[i].CloseIndex)
                {
                    isIntersected = true;
                    previousClosed[i].isIntersected = true;
                }
            
            previousClosed.Add(this);
        }

        foreach (var innerContext in InnerContexts) 
            innerContext.MarkIntersectedTags(previousClosed);
    }

    protected (int start, int end) CreateHtml(
        string text, 
        StringBuilder sb, 
        MarkDownEnvironment environment, 
        int nearestParentCloseIndex)
    {
        var start = StartIndex;
        var showInHtml = Closed && !HasUnsupportedParents(environment) && !isIntersected;

        if (showInHtml)
        {
            start += TagFactory.MarkDownOpen.Length;
            sb.Append(TagFactory.HtmlOpen);
        }
        
        foreach (var context in InnerContexts)
        {
            var beforeInner = sb.Length;
            var (innerStart, innerEnd) = context.CreateHtml(
                text, 
                sb, 
                environment, 
                showInHtml ? CloseIndex : nearestParentCloseIndex);
            
            sb.Insert(beforeInner, text.AsSpan(start, innerStart - start));
            start = innerEnd;
        }
        
        if (start < text.Length)
            sb.Append(text.AsSpan(start, (showInHtml ? CloseIndex : nearestParentCloseIndex) - start));
        
        if (showInHtml)
            sb.Append(TagFactory.HtmlClose);
        
        return (StartIndex, showInHtml ? CloseIndex + TagFactory.MarkDownClose.Length : nearestParentCloseIndex);
    }

    protected abstract void HandleSymbolItself(char symbol);

    public abstract void CloseSingleTags(int closeIndex);
}