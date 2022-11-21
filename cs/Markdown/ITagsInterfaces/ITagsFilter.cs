using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.Interfaces
{
    public interface ITagsFilter
    {
        public List<ITag> FilterTags(List<ITag> tags, string paragraph);
    }
}