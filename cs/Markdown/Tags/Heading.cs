using Markdown.Tokens;

namespace Markdown.Tags;

public class Heading : Tag
{
    public override bool IsPaired { get; protected set; } = false;
    public override string? ReplacementForOpeningTag { get; protected set; }
    public override string? ReplacementForClosingTag { get; protected set; }
    public override string? TagContent { get; set; }
    public override TagStatus Status { get; set; } = TagStatus.SelfClosing;
    public override TagType TagType { get; protected set; } =TagType.Heading;
    public override TagType[] ExcludedTags { get; protected set; } = Array.Empty<TagType>();

    protected override Tag CreateTag(string? content, Token? previousToken, string nextChar)
    {
        var nestingLevel = GetNestingLevel(content);
        ReplacementForOpeningTag = $"<h{nestingLevel}>";
        ReplacementForClosingTag = $"</h{nestingLevel}>";
        TagContent = content;
        BlockToken(previousToken);
        return this;
    }

    protected virtual void BlockToken(Token? previousToken)
    {
        if (previousToken is { Tag: not { TagType: TagType.Bulleted } })
            Status = TagStatus.Block;
    }

    private static int GetNestingLevel(string? content)
    {
        return content.Length - 1;
    }
}