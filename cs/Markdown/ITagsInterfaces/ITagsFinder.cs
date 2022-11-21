using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.Interfaces
{
    public interface ITagsFinder
    {
        public List<ITag> CreateTagList(string paragraph);
    }
}