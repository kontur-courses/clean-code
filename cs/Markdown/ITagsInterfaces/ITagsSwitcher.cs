using Markdown.TagClasses.ITagInterfaces;

namespace Markdown.ITagsInterfaces
{
    public interface ITagsSwitcher
    {
        public string SwitchTags(List<ITag> tags, string paragraph);
    }
}