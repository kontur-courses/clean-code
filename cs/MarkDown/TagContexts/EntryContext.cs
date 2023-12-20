using System.Text;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class EntryContext : TagContext
{
    public EntryContext(int startIndex, TagContext? context, Tag tag) : base(startIndex, context, tag)
    {
    }
    
    public EntryContext(Tag tag) : base(0, null, tag)
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
        CreateHtml(mdText, sb, environment, CloseIndex);
    }
}