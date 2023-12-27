using Markdown.Tokens;

namespace Markdown.Tags;

public class Bold : PairedTags
{
    public override bool IsPaired { get; protected set; }  = true;
    public override string? ReplacementForOpeningTag { get; protected set; } = "<strong>";
    public override string? ReplacementForClosingTag { get; protected set; } = "</strong>";
    public override string? TagContent { get; set; }
    public override TagStatus Status { get; set; } = TagStatus.Undefined;
    public override TagType TagType { get; protected set; }  = TagType.Bold;
    public override TagType[] ExcludedTags { get; protected set; } = Array.Empty<TagType>();

    protected override Tag CreateTag(string? content, Token? previousToken, string nextChar)
    {
        TagContent = content;
        PreviousToken = previousToken;

        OpenTag();
        IsTagWordBoundary(PreviousToken?.Content ?? "", nextChar);

        return this;
    }
}