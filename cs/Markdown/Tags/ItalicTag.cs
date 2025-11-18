namespace Markdown.Tags;

public class ItalicTag : BoundaryTag
{
    public override string MdTag => "_";
    public override string HtmlOpenTag => "<em>";
    public override string HtmlCloseTag => "</em>";

    protected override IReadOnlyList<Type> InvalidInnerTags =>
        [new StrongTag().GetType()];
}