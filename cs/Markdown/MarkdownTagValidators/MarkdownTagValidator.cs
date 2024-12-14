using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public class MarkdownTagValidator : IMarkdownTagValidator
{
    public bool IsValidSingleTag(
        List<IToken> paragraphOfTokens, 
        ISingleMarkdownTagToken singleTag)
    {
        return singleTag.TagType switch
        {
            MarkdownTagType.Heading or MarkdownTagType.MarkedList => IsValidSingleTag(singleTag.Token, paragraphOfTokens),
            _ => false
        };
    }

    public bool IsValidPairedTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTagTokens pairedTags, 
        MarkdownTagType? externalTagType = null)
    {
        return pairedTags.TagType switch
        {
            MarkdownTagType.Italics => IsValidItalicsTag(paragraphOfTokens, pairedTags),
            MarkdownTagType.Bold => IsValidBoldTag(paragraphOfTokens, pairedTags, externalTagType),
            _ => false
        };
    }
    
    private static bool IsValidSingleTag(TagToken tagToken, List<IToken> paragraphOfTokens)
    {
        var position = tagToken.PositionInTokens;

        if (paragraphOfTokens.Count < 2)
            return false;

        return position == 0 && paragraphOfTokens[position + 1].Type == TokenType.Space;
    }

    private static bool IsValidItalicsTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTagTokens pairedTags)
    {
        return MarkdownPairedTagsValidatorHelper.IsValidPairedTags(paragraphOfTokens, pairedTags);
    }

    private static bool IsValidBoldTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTagTokens pairedTags,
        MarkdownTagType? externalTagType)
    {
        if (pairedTags.Opening.PositionInTokens + 1 == pairedTags.Closing.PositionInTokens)
            return false;
        if (externalTagType is MarkdownTagType.Italics)
            return false;

        return MarkdownPairedTagsValidatorHelper.IsValidPairedTags(paragraphOfTokens, pairedTags);
    }
}