namespace Markdown.Tags;

public class StrongTag : BoundaryTag
{
    public override string MdTag => "__";
    public override string HtmlOpenTag => "<strong>";
    public override string HtmlCloseTag => "</strong>";
}