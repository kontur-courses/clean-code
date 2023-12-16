using System.Text;
using MarkDown.Enums;
using MarkDown.Interfaces;
using MarkDown.TagContexts;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDown
{
    // private readonly MarkDownEnvironment markDownEnvironment;
    // private EntryContext entryContext = new EntryContext(0); 
    // private readonly SortedSet<TagContext> openedContexts = new(new TagContextComparer());

    public MarkDown()
    {
        // this.markDownEnvironment = markDownEnvironment;
    }
    
    // public string GenerateHtml()
    // {
    //     var entryContext = new EntryContext(0, null);
    //     nowContext = entryContext;
    //     var sb = new StringBuilder();
    //
    //     for (var i = 1; i < text.Length; i++)
    //     {
    //         nowContext.HandleSymbol(text[i]);
    //         
    //         if (markDownEnvironment.CanGetTagCreator(i, out var openTag))
    //         {
    //             var newContext = openTag.CreateContext(i, nowContext);
    //             nowContext.AddInnerContext(nowContext);
    //             nowContext = newContext;
    //
    //             i += openTag.MarkDownClose.Length;
    //             continue;
    //         }
    //
    //         if (markDownEnvironment.CanGetCloseTag(text, i, out var closeTag))
    //         {
    //             if (nowContext.TryClose(closeTag.TagName, i))
    //             {
    //                 
    //                 i += closeTag.MarkDownClose.Length;
    //             }
    //         }
    //     }
    //
    //     entryContext.ConvertToHtml(markDownEnvironment, sb);
    //
    //     return sb.ToString();
    // }

    private static TagContext CreateContext(string mdText, MarkDownEnvironment environment)
    {
        var entryTag = new EntryTag(environment);
        var entryContext = entryTag.CreateContext();
        
        var nowContext = entryContext;
        
        for (var i = 0; i < mdText.Length; i++)
        {
            nowContext.HandleSymbol(mdText[i]);
            
            if (environment.CanGetTagCreator(mdText, i, out var openTag))
            {
                var newContext = openTag.CreateContext(i, nowContext);
                nowContext.AddInnerContext(newContext);
                
                nowContext = newContext;
            
                i += openTag.MarkDownOpen.Length - 1;
                continue;
            }

            if (environment.CanGetCloseTag(mdText, i, out var closeTags))
            {
                foreach (var closeTag in closeTags)
                {
                    nowContext.TryClose(closeTag.TagName, i);
                }
                
                i += closeTags.Max(e => e.MarkDownClose.Length);
            }
        }

        nowContext.CloseSingleTags(mdText.Length - 1);

        return entryContext;
    }
    
    public static string GenerateHtml(string mdText, MarkDownEnvironment environment)
    {
        var entryContext = CreateContext(mdText, environment);
        
        var sb = new StringBuilder();
        entryContext.ConvertToHtml(mdText, sb, environment);

        return sb.ToString();
    }
}