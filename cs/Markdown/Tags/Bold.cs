namespace Markdown.Tags;

public class Bold : PairedTags
{
    protected override Tag CreateTag(string content, Token? previousToken, string nextChar)
    {
        IsPaired = true;
        ReplacementForOpeningTag = "<strong>";
        ReplacementForClosingTag = "</strong>";
        TagType = TagType.Bold;
        TagContent = content;
        Status = TagStatus.Undefined;
        PreviousToken = previousToken;

        OpenTag(nextChar);
        IsTagWordBoundary(PreviousToken?.Content ?? "", nextChar);

        return this;
    }
}