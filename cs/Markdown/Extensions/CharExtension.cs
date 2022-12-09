using Markdown.Tag;
using System.Linq;

namespace Markdown.Extensions
{
    public static class CharExtension
    {
        public static bool IsTagStart<T>(this char ch) where T : ITag
        {
            return Tags.GetAll<T>()
                .Any(tag => tag.Opening.StartsWith(ch) || tag.Closing.StartsWith(ch));
        }

        public static bool IsEscapeCharacter(this char ch)
        {
            return ch == '\\';
        }

        public static bool IsNewLine(this char ch)
        {
            return ch == '\n';
        }
    }
}
