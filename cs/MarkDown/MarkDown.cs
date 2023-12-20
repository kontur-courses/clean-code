using System.Text;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDown
{
    private static EntryContext CreateContext(string mdText, MarkDownEnvironment environment)
    {
        var entryTag = new EntryTag(environment);
        var entryContext = entryTag.CreateContext();
        
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
                    if (nowContext.TryClose(closeTag.TagName, i, out var closedContext))
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
                var newContext = openTag.CreateContext(mdText, i, nowContext);
                nowContext.AddInnerContext(newContext);
                
                nowContext = newContext;
            
                i += openTag.MarkDownOpen.Length - 1;
                continue;
            }
        }

        nowContext.CloseSingleTags(mdText.Length);

        return entryContext;
    }
    
    public static string GenerateHtml(string mdText, MarkDownEnvironment environment)
    {
        var entryContext = CreateContext(mdText, environment);
        
        var sb = new StringBuilder();
        entryContext.CreateHtml(mdText, sb, environment);

        return sb.ToString();
    }
}