using Markdown.MarkdownTags;
using Markdown.Tokens.TagTokens;

namespace Markdown.Tokens.MarkdownTagTokens;

public class SingleMarkdownTagToken : ISingleMarkdownTagToken
{
    public SingleMarkdownTagToken(MarkdownTagType tagType, TagToken token)
    {
        Token = token;
        TagType = tagType;
    }

    public TagToken Token { get; }
    
    public MarkdownTag ConvertToMarkdownTag()
    {
        return new MarkdownTag(Token, TagType);
    }

    public MarkdownTagType TagType { get; }
    
    public static bool TryCreate(TagToken tagToken, out ISingleMarkdownTagToken? singleMarkdownTagToken)
    {
        switch (tagToken.Content)
        {
            case MarkdownTagContentConstants.Heading:
                singleMarkdownTagToken = new SingleMarkdownTagToken(MarkdownTagType.Heading, tagToken);
                return true;
            case MarkdownTagContentConstants.MarkedList:
                singleMarkdownTagToken = new SingleMarkdownTagToken(MarkdownTagType.MarkedList, tagToken);
                return true;
            default:
                singleMarkdownTagToken = null;
                return false;
        }
    }
}