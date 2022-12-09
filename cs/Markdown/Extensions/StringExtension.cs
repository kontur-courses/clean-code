using Markdown.Enums;
using Markdown.Tag;
using System;
using System.Linq;

namespace Markdown.Extensions
{
    public static class StringExtension
    {
        public static bool IsTag<T>(this string value) where T : ITag
        {
            return Tags.GetAll<T>()
                .Any(t => t.Opening == value || t.Closing == value);
        }

        public static bool IsTag<T>(this string value, TagState tagState) where T : ITag
        {
            Func<T, bool> condition = tagState switch
            {
                TagState.Opening => (t) => t.Opening == value,
                TagState.Closing => (t) => t.Closing == value,
                _ => default
            };
            return condition != null && Tags.GetAll<T>().Any(condition);
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
