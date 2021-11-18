using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public class HtmlTagStore : TagStore
    {
        private static readonly List<ITag> Tags = new()
        {
            new Tag(TagType.Emphasized, "<em>", "</em>"),
        };

        public HtmlTagStore()
        {
            Tags.ForEach(Register);
        }

        public override TagRole GetTagRole(string tag, char? before, char? after)
        {
            if (!IsTag(tag)) return TagRole.NotTag;
            return tag switch
            {
                "<em>" => TagRole.Opening,
                "</em>" => TagRole.Closing,
                _ => TagRole.NotTag
            };
        }
    }
}