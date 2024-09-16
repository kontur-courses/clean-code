using Markdown.Enums;
using Markdown.Interfaces;

namespace Markdown.Tags;

public class ListItem : IMarkdownTag
{
    public TagStatus Status { get; set; }
    public MarkdownContext MarkdownContext { get; set; }
    public TagType Type => TagType.ListItem;
    public MarkdownTagInfo Info => new("+", "<li>", "</li>");

    public void ChangeStatusIfBroken()
    {
        if (!TagsRepository.IsTagBroken(this))
            return;

        Status = TagStatus.Broken;
    }
}