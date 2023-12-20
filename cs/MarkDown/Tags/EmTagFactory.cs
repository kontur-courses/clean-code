using MarkDown.Enums;
using MarkDown.Tags.Abstracts;

namespace MarkDown.Tags;

public class EmTagFactory : HighlightTagFactory
{
    public EmTagFactory(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override TagName TagName => TagName.Em;
    public override string HtmlOpen => "<em>";
    public override string HtmlClose => "</em>";
    public override string MarkDownOpen => "_";
    public override string MarkDownClose => "_";
}