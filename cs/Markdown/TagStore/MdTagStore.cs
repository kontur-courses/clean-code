using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public class MdTagStore : TagStore
    {
        private static readonly List<ITag> Tags = new()
        {
            new Tag(TagType.Emphasized, "_", "_"),
            new Tag(TagType.Strong, "__", "__"),
        };

        public MdTagStore()
        {
            Tags.ForEach(Register);
        }

        public override TagRole GetTagRole(string text, int startIndex, int length)
        {
            char? before = startIndex == 0 ? null : text[startIndex - 1];
            char? after = startIndex + length == text.Length ? null : text[startIndex + length];
            if (before != ' ' && before != null)
                return after is ' ' or null ? TagRole.Closing : TagRole.NotTag;
            return after != ' ' ? TagRole.Opening : TagRole.NotTag;
        }
    }
}