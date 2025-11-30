namespace Markdown;

public class NoneMarkdownTag() : MarkdownTag(TagType.None, "", false)
{
    public override bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen)
    {
        return true;
    }
}