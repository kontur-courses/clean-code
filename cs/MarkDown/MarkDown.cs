using System.Text;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDown
{
    private static (EntryContext, IEnumerable<int>) CreateContext(string mdText, MarkDownEnvironment environment)
    {
        var entryTag = new EntryTagFactory(environment);
        var entryContext = entryTag.CreateContext();
        var isScreened = false;
        var screeningIndexes = new List<int>();
        
        TagContext nowContext = entryContext;
        
        for (var i = 0; i < mdText.Length; i++)
        {
            nowContext.HandleSymbol(mdText[i]);

            var (isClosed, closeLength, newOpenContext) = CloseContextIfPossible(
                mdText, i, nowContext, isScreened, environment);

            if (isClosed)
            {
                i += closeLength;
                continue;
            }

            var (isOpened, openLength, newContext) = OpenContextIfPossible(
                mdText, i, nowContext, isScreened, environment);

            if (isOpened)
            {
                i += openLength;
                nowContext = newContext;
                continue;
            }

            isScreened = HandleScreening(mdText, i, isScreened, screeningIndexes);
        }

        nowContext.CloseSingleTags(mdText.Length);

        return (entryContext, screeningIndexes);
    }

    private static (bool isClosed, int closeLength, TagContext newOpenContext) CloseContextIfPossible(
        string mdText, 
        int i, 
        TagContext nowContext,
        bool isScreened,
        MarkDownEnvironment environment)
    {
        var isAnyClosed = false;
        var newContext = nowContext;
        var maxCloseLength = 0;

        if (environment.CanGetCloseTags(mdText, i, out var closeTags))
        {
            foreach (var closeTag in closeTags)
            {
                if (nowContext.TryClose(closeTag.TagName, i, out var closedContext, isScreened))
                {
                    maxCloseLength = Math.Max(maxCloseLength, closeTag.MarkDownClose.Length);
                    isAnyClosed = true;

                    if (closedContext is ResetContext resetContext) 
                        newContext = resetContext.SwitchToOpenContext();
                }
            }
                
            if (isAnyClosed) 
                maxCloseLength = closeTags.Max(e => e.MarkDownClose.Length) - 1;
        }

        return (isAnyClosed, maxCloseLength, newContext);
    }

    private static (bool isOpened, int openLength, TagContext newContext) OpenContextIfPossible(
        string mdText,
        int i,
        TagContext nowContext,
        bool isScreened,
        MarkDownEnvironment environment)
    {
        if (environment.CanGetTagCreator(mdText, i, out var openTag))
        {
            var newContext = openTag.CreateContext(mdText, i, nowContext, isScreened);
            nowContext.AddInnerContext(newContext);
            
            return (true, openTag.MarkDownOpen.Length - 1, newContext);
        }

        return (false, 0, nowContext);
    }

    private static bool HandleScreening(
        string mdText,
        int i,
        bool isScreened,
        List<int> screeningIndexes)
    {
        if (mdText[i] != '\\') return false;
        if (!isScreened) return true;
        
        screeningIndexes.Add(i);
        return false;
    }
    
    public static string GenerateHtml(string mdText, MarkDownEnvironment environment)
    {
        var (entryContext, screeningIndexes) = CreateContext(mdText, environment);
        
        var sb = new StringBuilder();
        entryContext.CreateHtml(mdText, sb, environment, screeningIndexes);

        return sb.ToString();
    }
}