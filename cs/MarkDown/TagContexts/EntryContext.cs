using System.Text;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class EntryContext : TagContext
{
    public EntryContext(int startIndex, TagContext? context, TagFactory tagFactory, bool isScreened)
        : base(startIndex, context, tagFactory, isScreened)
    {
    }
    
    public EntryContext(TagFactory tagFactory) : base(0, null, tagFactory, false)
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

    public void CreateHtml(
        string mdText, 
        StringBuilder sb,
        MarkDownEnvironment environment, 
        IEnumerable<int> screeningIndexes)
    {
        MarkIntersectedTags(new List<TagContext>());
        CreateHtml(mdText, sb, environment, CloseIndex, screeningIndexes);
    }
}