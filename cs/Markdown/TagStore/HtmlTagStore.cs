using System.Collections.Generic;
using Markdown.Tags;

namespace Markdown.TagStore
{
    public class HtmlTagStore : TagStore
    {
        private static readonly List<ITag> Tags = new()
        {
            new Tag(TagType.Emphasized, "<em>", "</em>"),
            new Tag(TagType.Strong, "<strong>", "</strong>"),
            new Tag(TagType.Header, "<h1>", "</h1>"),
            new Tag(TagType.Image, "<img  src=\"", "\">"),
        };

        private static readonly Dictionary<string, TagRole> ValueToTagRole = new();

        public HtmlTagStore()
        {
            Tags.ForEach(Register);
        }

        private new void Register(ITag tag)
        {
            ValueToTagRole[tag.Opening] = TagRole.Opening;
            ValueToTagRole[tag.Closing] = TagRole.Closing;
            base.Register(tag);
        }

        public override TagRole GetTagRole(string text, int startIndex, int length)
        {
            var tag = text.Substring(startIndex, length);
            return IsTag(tag) ? ValueToTagRole[tag] : TagRole.NotTag;
        }
    }
}