using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public abstract class TagStore : ITagStore
    {
        private readonly Dictionary<TagType, ITag> typeToTag = new();
        private readonly Dictionary<string, ITag> stringToTag = new();

        protected void Register(ITag tag)
        {
            typeToTag[tag.Type] = tag;
            stringToTag[tag.Closing] = tag;
            if (tag.Opening != tag.Closing)
                stringToTag[tag.Opening] = tag;
        }


        public string GetTag(TagType type, TagRole role)
        {
            if (type == TagType.Escaped) return "";
            return role == TagRole.Opening ? typeToTag[type].Opening : typeToTag[type].Closing;
        }

        public TagType GetTagType(string text, int start, int length)
        {
            if (text[start] == '\\') return TagType.Escaped;
            return stringToTag[text.Substring(start, length)].Type;
        }

        public string[] GetTagsValues()
        {
            return stringToTag.Keys.ToArray();
        }

        public virtual TagRole GetTagRole(string text, int startIndex, int length)
        {
            return TagRole.NotTag;
        }

        protected bool IsTag(string value)
        {
            return stringToTag.ContainsKey(value);
        }
    }
}