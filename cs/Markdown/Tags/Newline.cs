using Markdown.Interfaces;
using Markdown.Enums;

namespace Markdown.Tags;

public class Newline : IMarkdownTag
{
    public TagStatus Status { get; set; }
    public MarkdownContext MarkdownContext { get; set; }
    public TagType Type => TagType.Newline;
    public MarkdownTagInfo Info => new("\\n", "<br/>", "</h1><br/>");

    public void ChangeStatusIfBroken()
    {
    }

    bool IMarkdownTag.IsClosingFor(IMarkdownTag another)
    {
        return another.Type == TagType.Header;
    }
}