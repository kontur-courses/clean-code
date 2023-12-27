namespace Markdown.Tags;

public abstract class PairedTags : Tag
{
    private static HashSet<string> wordBoundaries = new HashSet<string>()
    {
        " ",
        ".",
        "",
    };
    protected void OpenTag()
    {
        if (PreviousToken == null || PreviousToken.Content.Any(c => char.IsDigit(c) || char.IsWhiteSpace(c)))
            Status = TagStatus.Opening;
    }

    protected void IsTagWordBoundary(string content, string nextChar)
    {
        if ((wordBoundaries.Contains(nextChar) && (content == "" || content.Last() == ' ')) ||
            (PreviousToken is { Type: TokenType.Tag } && PreviousToken.Tag!.TagType == TagType))
        {
            Status = TagStatus.Block;
        }
        else if (wordBoundaries.Contains(nextChar) && !content.Any(char.IsDigit))
        {
            Status = TagStatus.Closing;
        }
    }
}