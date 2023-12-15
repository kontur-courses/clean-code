namespace Markdown;

public class Tag
{
    public readonly string TagName;
    public readonly int Position;

    public Tag(string tagName, int position)
    {
        TagName = tagName;
        Position = position;
    }

    public string BuildHtmlTag(bool isClosingTag)
    {
        throw new NotImplementedException();
    }
}