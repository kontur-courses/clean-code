using System.Text;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceSubstring(this string mainString, int position, string oldSubstring,
            string newSubstring)
        {
            var builder = new StringBuilder(mainString);
            builder.Replace(oldSubstring, newSubstring, position, oldSubstring.Length);
            return builder.ToString();
        }
    }
}