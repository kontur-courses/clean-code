using Markdown.Contracts;
using Markdown.Helpers;

namespace Markdown.Tags;

public class ItalicTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Italic;
    public TagInfo Info => new("_", "<em>", "</em>");

    public void ChangeStatusIfBroken()
    {
        if (!TagHelper.IsUnderscoreTagBroken(this))
            return;

        Status = TagStatus.Broken;
    }
}