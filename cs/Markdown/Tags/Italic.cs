using Markdown.Interfaces;
using Markdown.Enums;

namespace Markdown.Tags;

public class Italic : IMarkdownTag
{
    public TagStatus Status { get; set; }
    public MarkdownContext MarkdownContext { get; set; }
    public TagType Type => TagType.Italic;
    public MarkdownTagInfo Info => new("_", "<em>", "</em>");

    public void ChangeStatusIfBroken()
    {
        if (!TagsRepository.IsTagBroken(this))
            return;

        Status = TagStatus.Broken;
    }
}