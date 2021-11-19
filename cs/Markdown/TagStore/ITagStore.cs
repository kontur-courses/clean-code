using Markdown.Tags;

namespace Markdown.TagStore
{
    public interface ITagStore
    {
        string GetTag(TagType type, TagRole role);
        TagType GetTagType(string text, int start, int length);
        string[] GetTagsValues();
        public TagRole GetTagRole(string text, int startIndex, int length);
    }
}