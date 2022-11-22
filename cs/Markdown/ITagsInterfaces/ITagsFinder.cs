using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.ITagsInterfaces
{
    public interface ITagsFinder
    {
        public List<ITag> CreateTagList(string paragraph);
    }
}