using MarkDown.Enums;
using MarkDown.TagContexts;
using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class StrongTag : HighlightTag
{
    public StrongTag(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override TagName TagName => TagName.Strong;
    public override string HtmlOpen => "<strong>";
    public override string HtmlClose => "</strong>";
    public override string MarkDownOpen => "__";
    public override string MarkDownClose => "__";
    public override TagContext CreateContext(string mdText, int startIndex, TagContext parentContext)
    {
        var isInWord = startIndex > 0 && mdText[startIndex - 1] != ' ';

        return new StrongContext(startIndex, isInWord, parentContext, this);
    }
}