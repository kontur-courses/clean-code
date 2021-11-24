using System;
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
        };

        private static readonly HashSet<char?> AfterOpeningBanned = new() { ' ', null, '\\', '\n', '\r' };
        private static readonly HashSet<char?> BeforeClosingBanned = new() { ' ', null };
        public MdTagStore()
        {
            Tags.ForEach(Register);
        }

        public override TagRole GetTagRole(string text, int startIndex, int length)
        {
            char? before = startIndex == 0 ? null : text[startIndex - 1];
            char? after = startIndex + length == text.Length ? null : text[startIndex + length];
            if (AfterOpeningBanned.Contains(after))
            {
                if (BeforeClosingBanned.Contains(before))
                {
                    return TagRole.NotTag;
                }

                return TagRole.Closing;
            }

            if (!BeforeClosingBanned.Contains(before))
            {
                return TagRole.Undefined;
            }

            return TagRole.Opening;
        }
    }
}