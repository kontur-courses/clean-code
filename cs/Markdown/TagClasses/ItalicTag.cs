namespace Markdown.TagClasses;

public class ItalicTag : Tag
{
    public override string Name => "Italic";
    public override string MarkdownOpening => "_";
    public override string MarkdownClosing => "_";
    public override string HtmlTagOpen => "<em>";
    public override string HtmlTagClose => "</em>";
    public override bool ShouldHavePair => true;
    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return false;
    }
}