using MarkDown.ContextChanges;
using MarkDown.Interfaces;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDownContextCreator : IMarkDownContextCreator
{
    private readonly MarkDownEnvironment environment;

    public MarkDownContextCreator(MarkDownEnvironment environment)
    {
        this.environment = environment;
    }
    
    public IContextInfo GetFilledEntryContext(string mdText)
    {
        var entryTag = new EntryTagFactory();
        var entryContext = entryTag.CreateContext();
        var isScreened = false;
        var screeningIndexes = new List<int>();
        
        TagContext currentContext = entryContext;
        
        for (var i = 0; i < mdText.Length; i++)
        {
            var changes = GetContextChanges(mdText, i, currentContext, isScreened);
            
            if (!changes.HasChanges)
                continue;

            switch (changes)
            {
                case ContextChange contextChange:
                    currentContext = contextChange.NewContext;
                    i += contextChange.SkipIndexesCount;
                    break;
                case ScreeningChange screeningChange:
                {
                    isScreened = screeningChange.IsScreened;
                    if (screeningChange.NeedToAddIndex)
                        screeningIndexes.Add(i);
                    break;
                }
            }
        }

        currentContext.CloseSingleTags(mdText.Length);

        return new ContextInfo.ContextInfo(entryContext, screeningIndexes);
    }

    private IContextChange GetContextChanges(
        string mdText,
        int i,
        TagContext currentContext,
        bool isScreened
        )
    {
        currentContext.HandleSymbol(mdText[i]);
    
        var closeChanges = CloseContextIfPossible(
            mdText, i, currentContext, isScreened);

        if (closeChanges.HasChanges)
            return closeChanges;
    
        var openChanges = OpenContextIfPossible(
            mdText, i, currentContext, isScreened);

        if (openChanges.HasChanges)
            return openChanges;
    
        return HandleScreening(mdText, i, isScreened);
    }

    private IContextChange CloseContextIfPossible(
        string mdText, 
        int i, 
        TagContext currentContext,
        bool isScreened)
    {
        if (!environment.CanGetCloseTags(mdText, i, out var closeTags))
            return new ContextChange(false, currentContext, 0);
        
        var isAnyClosed = false;
        var newContext = currentContext;
        var maxCloseLength = 0;
        
        foreach (var closeTag in closeTags)
        {
            if (!currentContext.TryClose(closeTag.TagName, i, out var closedContext, isScreened)) continue;
            
            maxCloseLength = Math.Max(maxCloseLength, closeTag.MarkDownClose.Length);
            isAnyClosed = true;

            if (closedContext is ResetContext resetContext) 
                newContext = resetContext.SwitchToOpenContext();
        }
                
        if (isAnyClosed) 
            maxCloseLength = closeTags.Max(e => e.MarkDownClose.Length) - 1;

        return new ContextChange(isAnyClosed, newContext, maxCloseLength);
    }

    private IContextChange OpenContextIfPossible(
        string mdText,
        int i,
        TagContext currentContext,
        bool isScreened)
    {
        if (!environment.CanGetTagCreator(mdText, i, currentContext, out var openTag)) 
            return new ContextChange(false, currentContext, 0);
        
        var newContext = openTag.CreateContext(mdText, i, currentContext, isScreened);
        currentContext.AddInnerContext(newContext);
            
        return new ContextChange(true, newContext, openTag.SkipIndexesAfterCreating);

    }

    private IContextChange HandleScreening(
        string mdText,
        int i,
        bool isScreened)
    {
        if (mdText[i] != '\\') 
            return new ScreeningChange(true, false, false);
        
        if (!isScreened)
            return new ScreeningChange(true, true, false);
        
        return new ScreeningChange(true,false, true);
    }
}