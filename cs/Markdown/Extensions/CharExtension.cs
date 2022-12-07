using System.Linq;

namespace Markdown
{
    public static class CharExtension
    {
        public static bool IsTagStart(this char ch) => IsTagStart<ITag>(ch);
        public static bool IsTagStart<T>(this char ch) where T : ITag
        {
            return Tags.GetAll<T>()
                .Any(tag => tag.Opening.StartsWith(ch) || tag.Closing.StartsWith(ch));
        }
    }
}
