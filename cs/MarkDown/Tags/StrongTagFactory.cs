using MarkDown.Enums;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class StrongTagFactory : HighlightTagFactory
{
    public StrongTagFactory(MarkDownEnvironment environment)
    {
        environment.AddUnsupportedParentsFor(this, TagName.Em);
    }

    public override TagName TagName => TagName.Strong;
    public override string HtmlOpen => "<strong>";
    public override string HtmlClose => "</strong>";
    public override string MarkDownOpen => "__";
    public override string MarkDownClose => "__";
}