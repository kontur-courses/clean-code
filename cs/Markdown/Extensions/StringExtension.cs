using System;
using System.Linq;

namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsTag(this string tag) => IsTag<ITag>(tag);

        public static bool IsTag(this string tag, TagState tagState)
            => IsTag<ITag>(tag, tagState);

        public static bool IsTag<T>(this string tag) where T : ITag
        {
            return Tags.GetAll<T>()
                .Any(t => t.Opening == tag || t.Closing == tag);
        }

        public static bool IsTag<T>(this string tag, TagState tagState) where T : ITag
        {
            Func<T, bool> condition = default;
            switch (tagState)
            {
                case TagState.Opening: 
                    condition = (t) => t.Opening == tag; break;
                case TagState.Closing:
                    condition = (t) => t.Closing == tag; break;
            }
            return Tags.GetAll<T>().Any(condition);
        }

        public static bool IsTag<T>(this string value, T tag) where T : ITag
        {
            return tag.Opening == value || tag.Closing == value;
        }

        public static bool IsPossibleTag<T>(this string value) where T : ITag 
        {
            return Tags.GetAll<MarkdownTag>()
                .SelectMany(t => new[] { t.Opening, t.Closing })
                .Any(t => t.StartsWith(value));
        }
    }
}
