using Markdown.TagClasses;
using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.Interfaces
{
    public interface ITagsSwitcher
    {
        public string SwitchTags(List<ITag> tags, string paragraph);
    }
}