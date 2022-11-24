using System.Linq;

namespace Markdown.Tags
{
    public static class TagExtension
    {
        public static bool NeedTagIgnore(this ITag tag, ITag otherTag)
        {
            return tag.IgnoredTags.Any() && tag.IgnoredTags.Contains(otherTag);
        }
    }
}