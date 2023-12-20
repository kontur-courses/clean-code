namespace Markdown.Tags;

public class Italic : PairedTags
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = true;
        ReplacementForOpeningTag = "<em>";
        ReplacementForClosingTag = "</em>";
        ExcludedTags = new TagType[] { TagType.Bold };
        TagType = TagType.Italic;
        TagContent = content;
        Status = TagStatus.Undefined;
        PreviousToken = previousToken;

        OpenTag(nextChar);
        IsTagWordBoundary(PreviousToken?.Content ?? "", nextChar);

        return this;
    }
}