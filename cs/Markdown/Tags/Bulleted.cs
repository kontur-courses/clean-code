using Markdown.Tokens;

namespace Markdown.Tags;

public class Bulleted : Tag
{
    public override bool IsPaired { get; protected set; }
    public override string? ReplacementForOpeningTag { get; protected set; } = "<li>";
    public override string? ReplacementForClosingTag { get; protected set; }  = "</li>";
    public override string? TagContent { get; set; }
    public override TagStatus Status { get; set; } = TagStatus.SelfClosing;
    public override TagType TagType { get; protected set; } = TagType.Bulleted;
    public override TagType[] ExcludedTags { get; protected set; } = Array.Empty<TagType>();

    protected override Tag CreateTag(string? content, Token? previousToken, string nextChar)
    {
        TagContent = content;
        BlockToken(previousToken);
        return this;
    }

    private void BlockToken(Token? previousToken)
    {
        if (previousToken != null)
        {
            Status = TagStatus.Block;
        }
    }
}