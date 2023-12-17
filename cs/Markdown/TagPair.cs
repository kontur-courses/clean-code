namespace Markdown;

public class TagPair
{
    public MarkdownTagInfo FirstTagInfo;
    public MarkdownTagInfo SecondTagInfo;

    public TagPair(MarkdownTagInfo firstTagInfo, MarkdownTagInfo secondTagInfo)
    {
        FirstTagInfo = firstTagInfo;
        SecondTagInfo = secondTagInfo;
    }
}