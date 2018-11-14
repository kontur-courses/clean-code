using System.Linq;

namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsEscapeChar(this string markdown, int currentPosition)
        {
            return currentPosition >= 0 
                   && currentPosition < markdown.Length 
                   && markdown.ElementAt(currentPosition) == '\\';
        }

        public static string RemoveEscapes(this string markdown)
        {
            return markdown.Replace("\\", "");
        }

        public static bool IsWhiteSpace(this string markdown, int position)
        {
            return position >= 0
                   && position < markdown.Length 
                   && markdown.ElementAt(position) == ' ';
        }

    }
}
