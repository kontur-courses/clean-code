namespace Markdown.Tokens.Types;

public class TagPair
{
    public string OpeningTag { get; }
    public string ClosingTag { get; }

    public TagPair(string openingTag, string closingTag)
    {
        OpeningTag = openingTag;
        ClosingTag = closingTag;
    }
}