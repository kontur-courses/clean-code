using Markdown.Tags;

namespace Markdown.TagStore
{
    public interface ITagStore
    {
        string GetTag(TagType type, TagRole role);
        TagType GetTagType(string value);
        
        string[] GetTagsValues();
        public TagRole GetTagRole(string value, char? before, char? after);
    }
}