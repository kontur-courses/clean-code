using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public class MdTagStore : TagStore
    {
        private static readonly List<ITag> Tags = new()
        {
            new Tag(TagType.Emphasized, "_", "_"),
        };

        public MdTagStore()
        {
            Tags.ForEach(Register);
        }

        public override TagRole GetTagRole(string tag, char? before, char? after)
        {
            if (!IsTag(tag)) return TagRole.NotTag;
            
            if (before != ' ' && before != null)
                return after is ' ' or null ? TagRole.Closing : TagRole.NotTag;
            return after != ' ' ? TagRole.Opening : TagRole.NotTag;
        }
    }
}