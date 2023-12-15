using MarkDown.Abstracts;
using MarkDown.Interfaces;
using MarkDown.TagContexts;

namespace MarkDown.TagCreators;

public class Header : Tag
{
    public override string HtmlSyntax => "h1";
    public override string MarkDownOpen => "#";
    public override string MarkDownClose => System.Environment.NewLine;
    public override bool CanCancelContext => false;

    public Header(MarkDownEnvironment environment) : base(environment)
    {
    }

    public override ITagContext CreateContext(int startIndex)
    {
        return new HeaderContext(startIndex);
    }
}