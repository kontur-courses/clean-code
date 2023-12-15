using Markdown.Contracts;
using Markdown.Helpers;

namespace Markdown.Tags;

public class BoldTag : ITag
{
    public TagStatus Status { get; set; }
    public TagType Type => TagType.Bold;
    public TagInfo Info => new("__", "<strong>", "</strong>");
    public int ContainerPosition { get; init; }

    public void ChangeStatusIfBroken(string context)
    {
        if (!TagHelper.IsUnderscoreTagBroken(context, this))
            return;

        Status = TagStatus.Broken;
    }
}