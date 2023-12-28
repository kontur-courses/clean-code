namespace Markdown.Tags.MdTags;

public class BoldTag : PairTag
{
    public override string MdTag => "__";
    public override string HtmlTag => "<strong>";
}