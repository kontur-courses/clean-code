using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public class HeaderTag : Tag
{
    public HeaderTag() : base(new HeaderModel())
    {
    }

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
}