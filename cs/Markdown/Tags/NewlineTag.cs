using Markdown.Contracts;

namespace Markdown.Tags;

public class NewlineTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Newline;
    public TagInfo Info => new("\\n", string.Empty, string.Empty);

    // This tag can't be broken :)
    public void ChangeStatusIfBroken()
    {
    }
}