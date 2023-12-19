namespace Markdown.TagClasses;

public class HeaderTag : Tag
{
    public override string Name => "Header";
    public override string MarkdownOpening => "# ";
    public override string MarkdownClosing => null;
    public override string HtmlTagOpen => "<h1>";
    public override string HtmlTagClose => "</h1>";
    public override bool ShouldHavePair => true;

    public override bool IsMarkdownOpening(string markdownText, int startIndex)
    {
        return startIndex <= 0 || markdownText[startIndex-1].ToString() == "\n";
    }

    public override bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        return true;
    }

    public override bool CanBeOpened(string markdownText, int startIndex)
    {
        return IsMarkdownOpening(markdownText, startIndex);
    }

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