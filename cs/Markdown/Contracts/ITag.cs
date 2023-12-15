using Markdown.Tags;

namespace Markdown.Contracts;

public interface ITag
{
    public TagStatus Status { get; set; } 
    public TagType Type { get; }
    public TagInfo Info { get; }
    public int ContainerPosition { get; init; }
    public void ChangeStatusIfBroken(string context);
}