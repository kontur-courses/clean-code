using System.Text;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class EntryContext : ResetContext
{
    public EntryContext(int startIndex, TagContext? context, TagFactory tagFactory, bool isScreened)
        : base(startIndex, context, tagFactory, isScreened)
    {
    }
    
    public EntryContext(TagFactory tagFactory) : base(0, null, tagFactory, false)
    {
    }

    public string CreateHtml(
        string mdText, 
        MarkDownEnvironment environment, 
        IEnumerable<int> screeningIndexes)
    {
        var sb = new StringBuilder();
        MarkIntersectedTags(new List<TagContext>());
        CreateHtml(mdText, sb, environment, CloseIndex, screeningIndexes);

        return sb.ToString();
    }
}