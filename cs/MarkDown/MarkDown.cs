using System.Text;
using MarkDown.Interfaces;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDown : IMarkdown
{
    private readonly MarkDownEnvironment environment;
    private readonly IMarkDownContextCreator contextCreator;
    
    public MarkDown(MarkDownEnvironment environment, IMarkDownContextCreator contextCreator)
    {
        this.environment = environment;
        this.contextCreator = contextCreator;
    }
    
    public string RenderHtml(string mdText)
    {
        var contextInfo = contextCreator.GetFilledEntryContext(mdText);
        
        var sb = new StringBuilder();
        contextInfo.EntryContext.CreateHtml(mdText, sb, environment, contextInfo.ScreeningIndexes);

        return sb.ToString();
    }
}