using Markdown.Contracts;

namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public TagStatus Status { get; set; }
    public ContextInfo Context { get; set; }
    public TagType Type => TagType.Header;
    public TagInfo Info => new("# ", "<h1>", "</h1>");

    public void ChangeStatusIfBroken()
    {
        var text = Context.Text;
        var idx = Context.Position;

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