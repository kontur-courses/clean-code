namespace Markdown.Tags.TagSpecification;

public abstract class BaseTag(Tag opening, Tag closing)
{
    public Tag Opening { get; } = opening;
    public Tag Closing { get; } = closing;
    
    public virtual bool DidConflict(BaseTag tag) => false;
    
    public virtual TextFragment? FindNextPairOfTags(string text, int startIndex, BaseTag tagSpecification)
    {
        var opening = FindNextTag(text, startIndex, Opening, tagSpecification);
        if (opening is null) return null;

        startIndex = opening.StartIndex + Opening.Old.Length;
        var closing = FindNextTag(text, startIndex, Closing, tagSpecification);
        
        if (closing is null) return null;
        if (startIndex == closing.StartIndex)
            return FindNextPairOfTags(text, closing.StartIndex + closing.Tag.Old.Length, tagSpecification);
        
        return new TextFragment(opening, closing);
    }

    protected virtual TagReplacementSpecification? FindNextTag(string text, int startIndex, Tag tag, BaseTag tagSpecification)
    {
        var numberOfEscapeCharacters = 0;

        for (var i = startIndex; i <= text.Length - tag.Old.Length; i++)
        {
            if (text[i] == '\\') numberOfEscapeCharacters++;
            else if (numberOfEscapeCharacters % 2 == 0 && text.Substring(i, tag.Old.Length) == tag.Old &&
                     AdditionallyCheckCurrentPosition(text, i, tag))
            {
                return new TagReplacementSpecification(tag, tagSpecification, i);
            }
            else numberOfEscapeCharacters = 0;
        }

        return null;
    }

    public virtual string PerformTagFormatting(TagReplacementSpecification replacement,
        BaseTag? previousTag, BaseTag? nextTag)
    {
        return replacement.Tag == Opening ? Opening.New : Closing.New;
    }

    protected virtual bool AdditionallyCheckCurrentPosition(string text, int currentIndex, Tag tag) => true;
}