using MarkDown.Enums;
using MarkDown.Interfaces;
using MarkDown.TagContexts;

namespace MarkDown.Tags;

public class EntryTag : Tag
{
    public EntryTag(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override TagName TagName => TagName.Entry;
    public override string HtmlOpen => "";
    public override string HtmlClose => "";
    public override string MarkDownOpen => "";
    public override string MarkDownClose => "";
    
    public override TagContext CreateContext(int startIndex, TagContext? nowContext)
    {
        return new EntryContext(startIndex, nowContext, this);
    }
    
    public TagContext CreateContext()
    {
        return new EntryContext(this);
    }

    public override bool CanCreateContext(string text, int position)
    {
        return false;
    }

    public override bool IsClosePosition(string text, int position)
    {
        return text.Length == position + 1;
    }
}