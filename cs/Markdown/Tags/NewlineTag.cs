using Markdown.Contracts;

namespace Markdown.Tags;

public class NewlineTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Newline;
    public TagInfo Info => new("\\n", "<br/>", "</h1><br/>");

    public void ChangeStatusIfBroken()
    {
    }

    bool ITag.IsClosingFor(ITag another)
    {
        return another.Type == TagType.Header;
    }
}