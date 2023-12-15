using Markdown.Contracts;

namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public TagStatus Status { get; set; }
    public TagType Type => TagType.Header;
    public TagInfo Info => new("# ", "<h1>", "</h1>");
    public int ContainerPosition { get; init; }

    public void ChangeStatusIfBroken(string context)
    {
        if (context.Length == Info.GlobalMark.Length || ContainerPosition == 0)
            return;

        if (string.IsNullOrWhiteSpace(context[..ContainerPosition]))
            return;

        Status = TagStatus.Broken;
    }
}