namespace Markdown.TagClasses;

public class ListElement : Tag
{
    public override string Name => "ListElement";
    public override string MarkdownOpening => "\t";
    public override string MarkdownClosing => null;
    public override bool ShouldHavePair => true;
    public override string HtmlTagOpen => "<li>";
    public override string HtmlTagClose => "</li>";

    public override bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag, int otherTagEndIndex)
    {
        var isTypeOfTag = otherTag as NewLineTag;
        return isTypeOfTag != null;
    }

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return false;
    }
}