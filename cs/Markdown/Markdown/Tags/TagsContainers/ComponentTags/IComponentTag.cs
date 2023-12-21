namespace Markdown.Tags.TagsContainers.ComponentTags;

public interface IComponentTag : ITag
{
    public ITag BlockTag { get; }
}