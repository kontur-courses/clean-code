using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.ITagsInterfaces
{
    public interface ITagsFilter
    {
        public List<ITag> FilterTags(List<ITag> tags, string paragraph);
    }
}