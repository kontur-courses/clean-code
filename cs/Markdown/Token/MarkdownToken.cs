using Markdown.Tag;

namespace Markdown.Token;

public class MarkdownToken : IToken
{
    public TagType Type { get; }
    public int Position { get; }
    public int Length { get; }

    public MarkdownToken(int position, TagType type, int length)
    {
        Position = position;
        Type = type;
        Length = length;
    }
}