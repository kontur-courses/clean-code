using Markdown.TagClasses;

namespace Markdown.Interfaces
{
    public interface ITagsFilter
    {
        public List<TagInfo> FilterTags(List<TagInfo> tags, string paragraph);
    }
}