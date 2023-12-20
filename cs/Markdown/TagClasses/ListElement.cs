using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public class ListElement : Tag
{
    public ListElement() : base(new ListElementModel())
    {
    }

    public override bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag, int otherTagEndIndex)
    {
        var isTypeOfTag = otherTag as NewLineTag;
        return isTypeOfTag != null;
    }
}