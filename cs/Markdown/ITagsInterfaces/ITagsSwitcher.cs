using Markdown.TagClasses;

namespace Markdown.Interfaces
{
    public interface ITagsSwitcher
    {
        public string SwitchTags(List<TagInfo> tags, string paragraph);
    }
}