using Markdown.MarkdownTags;
using Markdown.Tokens;
using Markdown.Tokens.MarkdownTagTokens;
using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTagValidators;

public interface IMarkdownTagValidator
{
    public bool IsValidSingleTag(
        List<IToken> paragraphOfTokens,
        ISingleMarkdownTagToken singleTag);

    public bool IsValidPairedTag(
        List<IToken> paragraphOfTokens, 
        IPairedMarkdownTagTokens pairedTags,
        MarkdownTagType? externalTagType = null);
}