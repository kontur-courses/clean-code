namespace Markdown.Tags;

public class  Bold : PairedTags
{
    protected override Tag CreateTag(string content, Token previousToken, string nextChar)
    {
        IsPaired = true;
        ConvertTo = "<strong>";
        ClosingTag = "</strong>";
        TokenType = TagType.Bold;
        Content = content;
        Status = TagStatus.Undefined;
        PreviousToken = previousToken;
        OpenTag(nextChar);
        var c = PreviousToken != null ? previousToken.Content : "";
        IsTagWordBoundary(c , nextChar);
        return this;
    }
}