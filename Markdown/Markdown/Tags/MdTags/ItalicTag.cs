namespace Markdown.Tags.MdTags;

public class ItalicTag : PairTag
{
    public override string MdTag => "_";
    public override string HtmlTag => "<em>";
    protected override IEnumerable<PairTag> ProhibitedInside => new PairTag[] {new BoldTag()};
}