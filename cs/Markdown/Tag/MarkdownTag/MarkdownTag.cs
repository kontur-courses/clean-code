namespace Markdown;

public abstract class MarkdownTag(TagType tagType, string tagText, bool isPairedTag, int? tagLength = null)
{
    public TagType TagType { get; } = tagType;
    public string TagText { get; } = tagText;
    public bool IsPairedTag { get; } = isPairedTag;
    public int TotalTagLength { get; } = tagLength ?? tagText.Length;
    
    public abstract bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen);

    public virtual bool IsTagCorrect(string text, int endPosition, OpenToken openToken)
    {
        return true;
    }
}