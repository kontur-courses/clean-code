using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public abstract class TagStore : ITagStore
    {
        public Dictionary<TagType, string> tagToString = new();
        public Dictionary<string, TagType> stringToTag = new();

        public void Register(ITag tag)
        {
            tagToString[tag.Type] = tag.Value;
            stringToTag[tag.Value] = tag.Type;
        }

        public string ToString(TagType type)
        {
            throw new System.NotImplementedException();
        }

        public TagType ToTag(string value)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetTagsValues()
        {
            throw new System.NotImplementedException();
        }
    }
}