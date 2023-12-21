using Markdown.Interfaces;
using Markdown.Enums;

namespace Markdown.Tags;

public class Bold : IMarkdownTag
{
    public TagStatus Status { get; set; }
    public MarkdownContext MarkdownContext { get; set; }
    public TagType Type => TagType.Bold;
    public MarkdownTagInfo Info => new("__", "<strong>", "</strong>");

    public void ChangeStatusIfBroken()
    {
        if (!TagsRepository.IsTagBroken(this))
            return;

        Status = TagStatus.Broken;
    }
}