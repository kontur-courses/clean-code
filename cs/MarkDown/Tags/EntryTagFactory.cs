using MarkDown.Enums;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class EntryTagFactory : TagFactory
{
    public EntryTagFactory(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override TagName TagName => TagName.Entry;
    public override string HtmlOpen => "";
    public override string HtmlClose => "";
    public override string MarkDownOpen => "";
    public override string MarkDownClose => "";
    
    public override TagContext CreateContext(string mdText, int startIndex, TagContext? nowContext)
    {
        return new EntryContext(startIndex, nowContext, this);
    }
    
    public EntryContext CreateContext()
    {
        return new EntryContext(this);
    }

    public override bool CanCreateContext(string text, int position)
    {
        return false;
    }

    public override bool IsClosePosition(string text, int position)
    {
        return false;
    }
}