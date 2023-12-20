using System.Text;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class EntryContext : TagContext
{
    public EntryContext(int startIndex, TagContext? context, TagFactory tagFactory) : base(startIndex, context, tagFactory)
    {
    }
    
    public EntryContext(TagFactory tagFactory) : base(0, null, tagFactory)
    {
    }

    protected override void HandleSymbolItself(char symbol)
    {
    }

    public override void CloseSingleTags(int closeIndex)
    {
        Closed = true;
        CloseIndex = closeIndex;
    }

    public void CreateHtml(string mdText, StringBuilder sb, MarkDownEnvironment environment)
    {
        MarkIntersectedTags(new List<TagContext>());
        CreateHtml(mdText, sb, environment, CloseIndex);
    }
}