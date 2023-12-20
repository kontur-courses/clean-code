using MarkDown.Enums;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class HeaderTagFactory : TagFactory
{
    public override TagName TagName => TagName.Header;

    public override string HtmlOpen => "<h1>";
    public override string HtmlClose => "</h1>";
    public override string MarkDownOpen => "# ";
    public override string MarkDownClose => System.Environment.NewLine;

    public HeaderTagFactory(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override HeaderContext CreateContext(string mdText, int startIndex, TagContext nowContext, bool isScreened)
    {
        return new HeaderContext(startIndex, nowContext, this, isScreened);
    }

    public override bool CanCreateContext(string text, int position, TagContext parentContext)
    {
        if (position > 0 && text[position - 1].ToString() != System.Environment.NewLine)
            return false;
        
        return text.Substring(position, MarkDownOpen.Length).Equals(MarkDownOpen);
    }

    public override bool IsClosePosition(string text, int position)
    {
        return text[position].ToString().Equals(System.Environment.NewLine);
    }
}
