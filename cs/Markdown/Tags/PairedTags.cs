namespace Markdown.Tags;

public abstract class PairedTags : Tag
{
    protected void OpenTag(string nextChar)
    {
        if (PreviousToken == null || PreviousToken.Content.Any(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
            Status = TagStatus.Opening;
    }

    protected void IsTagWordBoundary(string content, string nextChar)
    {
        if ((nextChar is "" or " " && (content == "" || content.Last() == ' ')) ||
            (PreviousToken is { Type: Markdown.TokenType.Tag } && PreviousToken.Tag.TagType == TagType))
            Status = TagStatus.Block;
        else if (nextChar is "" or " " && !content.Any(char.IsDigit)) Status = TagStatus.Closing;
    }
}