namespace Markdown.Tags;

public class Italic : PairedTags
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = true;
        ConvertTo = "<em>";
        ClosingTag = "</em>";
        BlockTags = new TagType[]{TagType.Bold};
        TokenType = TagType.Italic;
        Content = content;
        Status = TagStatus.Undefined;
        PreviousToken = previousToken;
        OpenTag(nextChar);
        var c = PreviousToken != null ? previousToken.Content : "";
        IsTagWordBoundary(c, nextChar);
        return this;
    }
}