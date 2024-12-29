namespace Markdown.Tags;

public class Bold(string mdText, int tagStart) : PairTag(mdText, tagStart)
{
    protected override string MdTag => "__";
    protected override string HtmlTag => "strong";

    public override MdTagType TagType => MdTagType.Bold;

    public override void TryCloseTag(int contextEnd, string sourceMdText, out int tagEnd, List<Tag>? nested = null)
    {
        tagEnd = contextEnd + MdTag.Length;
        if (contextEnd != TagStart + MdTag.Length)
        {
            base.TryCloseTag(contextEnd, sourceMdText, out tagEnd, nested);
        }
    }
}