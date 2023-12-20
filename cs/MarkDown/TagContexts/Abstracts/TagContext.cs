using System.Text;
using MarkDown.Enums;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts.Abstracts;

public abstract class TagContext
{
    protected List<TagContext> InnerContexts = new();
    protected internal readonly TagContext? parent;

    protected TagContext(int startIndex, TagContext? parent, Tag tag)
    {
        this.parent = parent;
        StartIndex = startIndex;
        Tag = tag;
    }

    public int StartIndex { get; init; }
    protected Tag Tag { get; }
    public int CloseIndex { get; protected set; }
    
    public bool Closed { get; protected set; }
    
    public bool ConsiderInCreatingHtml { get; protected set; } = true;

    public bool TryClose(TagName tagCloseName, int closeIndex, out TagContext closed)
    {
        closed = this;
        
        if (parent != null && parent.TryClose(tagCloseName, closeIndex, out closed))
            return true;
        
        if (Closed)
            return false;
        
        Closed = Tag.TagName == tagCloseName && ConsiderInCreatingHtml;
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

    protected (int start, int end) CreateHtml(
        string text, 
        StringBuilder sb, 
        MarkDownEnvironment environment, 
        int nearestParentCloseIndex)
    {
        var start = StartIndex;

        if (Closed)
        {
            start += Tag.MarkDownOpen.Length;
            sb.Append(Tag.HtmlOpen);
        }
        
        foreach (var context in InnerContexts)
        {
            var beforeInner = sb.Length == 0 ? sb.Length : sb.Length - 1;
            var (innerStart, innerEnd) = context.CreateHtml(
                text, 
                sb, 
                environment, 
                Closed ? CloseIndex : nearestParentCloseIndex);
            
            sb.Insert(beforeInner, text.AsSpan(start, innerStart - start));
            start = innerEnd;
        }
        
        if (start < text.Length)
            sb.Append(text.AsSpan(start, (Closed ? CloseIndex : nearestParentCloseIndex) - start));
        
        if (Closed)
            sb.Append(Tag.HtmlClose);
        
        return (StartIndex, Closed ? CloseIndex + Tag.MarkDownClose.Length : nearestParentCloseIndex);
    }

    protected abstract void HandleSymbolItself(char symbol);

    public abstract void CloseSingleTags(int closeIndex);

    
    //todo убрать
    public override int GetHashCode()
    {
        unchecked
        {
            return ((int) Tag.TagName * 397) ^ StartIndex;
        }
    }
}