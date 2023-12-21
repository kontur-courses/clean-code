using Markdown.Interfaces;
using Markdown.Enums;

namespace Markdown.Tags;

public class Header : IMarkdownTag
{
    public TagStatus Status { get; set; }
    public MarkdownContext MarkdownContext { get; set; }
    public TagType Type => TagType.Header;
    public MarkdownTagInfo Info => new("# ", "<h1>", "</h1>");

    public void ChangeStatusIfBroken()
    {
        var text = MarkdownContext.Text;
        var idx = MarkdownContext.Position;

        while (--idx > -1)
        {
            if (char.IsWhiteSpace(text[idx]))
                continue;

            if (string.Concat(text[idx - 1], text[idx]) != "\\n")
                break;

            var count = 0;

            idx -= 2;

            while (idx > -1 && text[idx--] == '\\')
                count++;

            if (count % 2 == 1)
                break;

            return;
        }

        if (idx > -1)
            Status = TagStatus.Broken;
    }
}