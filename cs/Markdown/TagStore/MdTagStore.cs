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
            new Tag(TagType.Header, "#", "\r\n"),
            new Tag(TagType.Header, "#", "\n"),
            new Tag(TagType.Header, "#", "\n"),
        };

        private static readonly HashSet<char?> AfterOpeningBanned = new() { ' ', null, '\\', '\n', '\r' };
        private static readonly HashSet<char?> BeforeClosingBanned = new() { ' ', null };

        public MdTagStore()
        {
            Tags.ForEach(Register);
        }

        public override TagRole GetTagRole(string text, int startIndex, int length)
        {
            char? before = startIndex > 0 ? text[startIndex - 1] : null;
            char? after = startIndex + length < text.Length ? text[startIndex + length] : null;
            if (AfterOpeningBanned.Contains(after))
            {
                return BeforeClosingBanned.Contains(before)
                    ? TagRole.NotTag
                    : TagRole.Closing;
            }

            return BeforeClosingBanned.Contains(before)
                ? TagRole.Opening
                : TagRole.Undefined;
        }
    }
}