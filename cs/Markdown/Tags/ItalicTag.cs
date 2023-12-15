using Markdown.Contracts;
using Markdown.Helpers;

namespace Markdown.Tags;

public class ItalicTag : ITag
{
    public TagStatus Status { get; set; }
    public TagType Type => TagType.Italic;
    public TagInfo Info => new("_", "<em>", "</em>");
    public int ContainerPosition { get; init; }

    public void ChangeStatusIfBroken(string context)
    {
        if (!TagHelper.IsUnderscoreTagBroken(context, this))
            return;

        Status = TagStatus.Broken;
    }
}