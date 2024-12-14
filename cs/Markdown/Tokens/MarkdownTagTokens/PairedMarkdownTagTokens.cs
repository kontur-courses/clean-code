using Markdown.MarkdownTags;
using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public class PairedMarkdownTagTokens : IPairedMarkdownTagTokens
{
    public PairedMarkdownTagTokens(MarkdownTagType tagType, TagToken opening, TagToken closing)
    {
        TagType = tagType;
        Opening = opening;
        Closing = closing;
    }

    public MarkdownTagType TagType { get; }
    
    public TagToken Opening { get; }
    
    public TagToken Closing { get; }

    public IEnumerable<MarkdownTag> ConvertToMarkdownTags()
    {
        yield return new MarkdownTag(Opening, TagType);
        yield return new MarkdownTag(Closing, TagType, true);
    }

    public bool IsIntersect(IPairedMarkdownTagTokens pairedMarkdownTagTokens)
    {
        if (IsExternalFor(pairedMarkdownTagTokens) || pairedMarkdownTagTokens.IsExternalFor(this))
        {
            return false;
        }

        return (pairedMarkdownTagTokens.Opening.PositionInTokens < Closing.PositionInTokens &&
                Closing.PositionInTokens < pairedMarkdownTagTokens.Closing.PositionInTokens) || 
               (Opening.PositionInTokens < pairedMarkdownTagTokens.Closing.PositionInTokens && 
                pairedMarkdownTagTokens.Closing.PositionInTokens < Closing.PositionInTokens);
    }

    public bool IsExternalFor(IPairedMarkdownTagTokens pairedMarkdownTagTokens)
    {
        return Opening.PositionInTokens < pairedMarkdownTagTokens.Opening.PositionInTokens &&
               pairedMarkdownTagTokens.Closing.PositionInTokens < Closing.PositionInTokens;
    }

    public static bool TryCreate(
        TagToken opening,
        TagToken closing,
        out IPairedMarkdownTagTokens? pairedMarkdownTagTokens)
    {
        if (opening.Content != closing.Content)
        {
            pairedMarkdownTagTokens = null;
            return false;
        }

        switch (opening.Content)
        {
            case MarkdownTagContentConstants.Bold:
                pairedMarkdownTagTokens = new PairedMarkdownTagTokens(MarkdownTagType.Bold, opening, closing);
                return true;
            case MarkdownTagContentConstants.Italics:
                pairedMarkdownTagTokens = new PairedMarkdownTagTokens(MarkdownTagType.Italics, opening, closing);
                return true;
            default:
                pairedMarkdownTagTokens = null;
                return false;
        }
    }
}