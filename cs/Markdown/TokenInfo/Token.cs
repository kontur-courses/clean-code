using Markdown.TagInfo;

namespace Markdown.TokenInfo;

public class Token(Tag tag, int position, TagType tagType, int tagLength)
{
    public readonly int Position = position;
    public readonly Tag Tag = tag;
    public readonly TagType TagType = tagType;
    public readonly int TagLength = tagLength;
}