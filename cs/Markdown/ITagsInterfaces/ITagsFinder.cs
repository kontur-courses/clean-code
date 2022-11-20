using Markdown.TagClasses;

namespace Markdown.Interfaces
{
    public interface ITagsFinder
    {
        public List<TagInfo> CreateTagList(string paragraph);
    }
}