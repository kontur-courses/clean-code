using Markdown.Contracts;
using Markdown.Helpers;

namespace Markdown.Tags;

public class BoldTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Bold;
    public TagInfo Info => new("__", "<strong>", "</strong>");

    public void ChangeStatusIfBroken()
    {
        if (!TagHelper.IsUnderscoreTagBroken(this))
            return;

        Status = TagStatus.Broken;
    }
}