using Markdown.Tokens;

namespace Markdown.Tags;

public class Italic : PairedTags
{
    public override bool IsPaired { get; protected set; }  = true;
    public override string? ReplacementForOpeningTag { get; protected set; } = "<em>";
    public override string? ReplacementForClosingTag { get; protected set; } = "</em>";
    public override string? TagContent { get; set; } 
    public override TagStatus Status { get; set; }  = TagStatus.Undefined;
    public override TagType TagType { get; protected set; } = TagType.Italic;
    public override TagType[] ExcludedTags { get; protected set; } = new [] { TagType.Bold };

    protected override Tag CreateTag(string? content, Token? previousToken, string nextChar)
    {
        TagContent = content;
        PreviousToken = previousToken;

        OpenTag();
        IsTagWordBoundary(PreviousToken?.Content ?? "", nextChar);

        return this;
    }
}