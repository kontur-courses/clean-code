using System.Linq;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool ContainsAt(this string s, int index, string substring)
        {
            if (substring.Length > s.Length - index)
                return false;
            return !substring.Where((t, i) => s[index + i] != t).Any();
        }
    }
}