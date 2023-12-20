using System.Text;
using MarkDown.Enums;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts.Abstracts;

public abstract class TagContext
{
    private readonly List<TagContext> innerContexts = new();
    protected internal readonly TagContext? Parent;

    protected TagContext(int startIndex, TagContext? parent, TagFactory tagFactory, bool isScreened)
    {
        Parent = parent;
        StartIndex = isScreened ? startIndex - 1 : startIndex;
        TagFactory = tagFactory;
        IsScreenedStart = isScreened;
    }

    public bool Closed { get; protected set; }
    protected int CloseIndex { get; set; }
    protected bool ConsiderInCreatingHtml { get; set; } = true;
    private int StartIndex { get; init; }
    private TagFactory TagFactory { get; }
    private bool IsIntersected { get; set; }
    private bool IsScreenedStart { get; }
    private bool IsScreenedEnd { get; set; }
    private bool IsScreened => IsScreenedEnd || IsScreenedStart;
    
    public bool TryClose(TagName tagCloseName, int closeIndex, out TagContext closed, bool isScreened)
    {
        closed = this;
        
        if (Parent != null && Parent.TryClose(tagCloseName, closeIndex, out closed, isScreened))
            return true;
        
        if (Closed)
            return false;

        var canClose = TagFactory.TagName == tagCloseName && ConsiderInCreatingHtml;

        if (canClose)
        {
            Closed = canClose;
            CloseIndex = closeIndex;
            IsScreenedEnd = isScreened;
        }
        
        return canClose;
    }

    public void AddInnerContext(TagContext tagContext)
    {
        if (!Closed)
        {
            innerContexts.Add(tagContext);
            return;
        }
        
        Parent?.AddInnerContext(tagContext);
    }

    public void HandleSymbol(char symbol)
    {
        HandleSymbolItself(symbol);
        Parent?.HandleSymbol(symbol);
    }

    private bool HasUnsupportedParents(MarkDownEnvironment environment)
    {
        if (!Closed)
            return false;
        
        var nowParent = Parent;
        var unsupported = environment.GetUnsupportedParentsFor(TagFactory);

        while (nowParent is not null)
        {
            if (nowParent is {Closed: true, IsScreened: false}
                && nowParent.CloseIndex > StartIndex
                && unsupported.Any(e => e.Equals(nowParent.TagFactory.TagName)))
                return true;
            
            nowParent = nowParent.Parent;
        }
        
        return false;
    }

    private protected void MarkIntersectedTags(List<TagContext> previousClosed)
    {
        if (Closed && !IsScreened && this is HighlightContext)
        {
            for (var i = previousClosed.Count - 1; i >= 0; i--)
                if (previousClosed[i].CloseIndex > StartIndex && CloseIndex > previousClosed[i].CloseIndex)
                {
                    IsIntersected = true;
                    previousClosed[i].IsIntersected = true;
                }
            
            previousClosed.Add(this);
        }

        foreach (var innerContext in innerContexts) 
            innerContext.MarkIntersectedTags(previousClosed);
    }

    protected (int start, int end) CreateHtml(
        string text, 
        StringBuilder sb, 
        MarkDownEnvironment environment, 
        int nearestParentCloseIndex,
        IEnumerable<int> screeningIndexes)
    {
        var showInHtml = Closed && !HasUnsupportedParents(environment) && !IsIntersected;
        var start = AppendStart(showInHtml, sb);
        
        foreach (var context in innerContexts)
        {
            var beforeInner = sb.Length;
            var (innerStart, innerEnd) = context.CreateHtml(
                text, sb, environment, 
                showInHtml ? CloseIndex : nearestParentCloseIndex,
                screeningIndexes);

            foreach (var part in GetPartsWithoutScreening(start, innerStart, screeningIndexes)) 
                sb.Insert(beforeInner, text.AsSpan(part.start, part.end - part.start));
            start = innerEnd;
        }
        
        if (start < text.Length)
            foreach (var part in GetPartsWithoutScreening(
                         start, 
                         showInHtml ? CloseIndex : nearestParentCloseIndex,
                         screeningIndexes))
                sb.Append(text.AsSpan(part.start, part.end - part.start));

        var end = AppendEnd(showInHtml, nearestParentCloseIndex, sb);
        
        return (StartIndex, end);
    }

    private int AppendStart(bool showInHtml, StringBuilder sb)
    {
        var start = StartIndex;
        
        if (IsScreened)
        {
            start += IsScreenedStart ? 1 : 0;
            start += TagFactory.MarkDownOpen.Length;
            sb.Append(TagFactory.MarkDownOpen);
        }
        
        if (showInHtml && !IsScreened)
        {
            start += TagFactory.MarkDownOpen.Length;
            sb.Append(TagFactory.HtmlOpen);
        }

        return start;
    }

    private int AppendEnd(bool showInHtml, int nearestParentCloseIndex, StringBuilder sb)
    {
        if (showInHtml)
        {
            if (IsScreened)
            {
                if (IsScreenedEnd)
                {
                    CloseIndex++;
                    nearestParentCloseIndex++;
                }
                
                sb.Append(TagFactory.MarkDownClose);
            }
            else
                sb.Append(TagFactory.HtmlClose);
        }

        return showInHtml
            ? CloseIndex + TagFactory.MarkDownClose.Length
            : nearestParentCloseIndex;
    }

    private IEnumerable<(int start, int end)> GetPartsWithoutScreening(int start, int end, IEnumerable<int> screeningIndexes)
    {
        var nowStart = start;
        var suitable = screeningIndexes.Where(e => e > start && e < end).ToArray();

        for (var i = 0; i < suitable.Length; i++)
        {
            yield return (nowStart, suitable[i]);
            nowStart = suitable[i] + 1;
        }
        
        yield return (nowStart, end);
    }

    protected abstract void HandleSymbolItself(char symbol);

    public abstract void CloseSingleTags(int closeIndex);
}