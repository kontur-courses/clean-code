using System.Linq;

namespace Markdown.Extensions
{
    public static class StringUtils
    {
        public static bool IsWord(string text) => text.All(char.IsLetterOrDigit);
    }
}