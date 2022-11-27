using System.Linq;
using Markdown.Tags;

namespace Markdown.Extensions
{
    public static class TagExtension
    {
        public static bool NeedTagIgnore(this ITag tag, ITag otherTag)
        {
            return tag.IgnoredTags.Any() && tag.IgnoredTags.Contains(otherTag);
        }
        
        public static bool IsNumberInHighlightingTag(this ITag tag, char symbol)
        {
            return char.IsDigit(symbol) && Tag.IsHighlightingTag(tag);
        }
    }
}