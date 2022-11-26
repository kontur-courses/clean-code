using Markdown.Enums;

namespace Markdown.Tokens;

public class Tag : Token
{
    public Tag(int start, int end, TokenType type, TagStatus status) : base(start, end, type)
    {
        Status = status;
    }

    public TagStatus Status { get; set; }
}