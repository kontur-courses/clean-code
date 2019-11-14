namespace Markdown
{
    public class MarkdownSeparatorHandler
    {
        public static bool IsSeparator(string text, int position)
        {
            return text[position] == '_';
        }

        public static string GetSeparator(string text, int position)
        {
            return text[position].ToString();
        }
    }
}