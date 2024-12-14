using Markdown.Tokens.TagTokens;

namespace Markdown.MarkdownTags;

public class MarkdownTag
{
    public MarkdownTag(TagToken token, MarkdownTagType tagType, bool isClosedTag = false)
    {
        Token = token;
        TagType = tagType;
        IsClosedTag = isClosedTag;
    }

    public MarkdownTagType TagType { get; }
    
    // Токен соответствующий данному тегу
    public TagToken Token { get; }
    
    // Если тэг закрывающий
    public bool IsClosedTag { get; }
}