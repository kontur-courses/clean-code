namespace Markdown.TagClasses;

public class BoldTag : Tag
{
    public override string Name => "Bold";
    public override string MarkdownOpening => "__";
    public override string MarkdownClosing => "__";
    public override string HtmlTagOpen => "<strong>";
    public override string HtmlTagClose => "</strong>";
    public override bool ShouldHavePair => true;

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return tagsContext.Select(tag => tag.GetType()).Contains(typeof(ItalicTag));
    }
}