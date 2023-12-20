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
            
            if (environment.CanGetCloseTags(mdText, i, out var closeTags))
            {
                var maxCloseLength = 0;
                var isAnyClosed = false;
                    
                foreach (var closeTag in closeTags)
                {
                    if (nowContext.TryClose(closeTag.TagName, i, out var closedContext, isScreened))
                    {
                        maxCloseLength = Math.Max(maxCloseLength, closeTag.MarkDownClose.Length);
                        isAnyClosed = true;

                        if (closedContext is ResetContext resetContext)
                        {
                            nowContext = resetContext.SwitchToOpenContext();
                        }
                    }
                }
                
                if (isAnyClosed)
                {
                    i += closeTags.Max(e => e.MarkDownClose.Length) - 1;
                    continue;
                }
            }
            
            if (environment.CanGetTagCreator(mdText, i, out var openTag))
            {
                var newContext = openTag.CreateContext(mdText, i, nowContext, isScreened);
                nowContext.AddInnerContext(newContext);
                
                nowContext = newContext;
            
                i += openTag.MarkDownOpen.Length - 1;
                continue;
            }
            
            if (mdText[i] == '\\')
            {
                if (isScreened)
                {
                    screeningIndexes.Add(i);
                    isScreened = false;
                }
                else
                    isScreened = true;
            }
            else
                isScreened = false;
        }

        nowContext.CloseSingleTags(mdText.Length);

        return (entryContext, screeningIndexes);
    }
    
    public static string GenerateHtml(string mdText, MarkDownEnvironment environment)
    {
        var (entryContext, screeningIndexes) = CreateContext(mdText, environment);
        
        var sb = new StringBuilder();
        entryContext.CreateHtml(mdText, sb, environment, screeningIndexes);

        return sb.ToString();
    }
}