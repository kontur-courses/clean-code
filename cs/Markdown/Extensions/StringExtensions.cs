namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static char[] GetNeighborsOfSymbol(this string text, int position)
        {
            if (text.Length == 1) return new char[0];
            if (position > 0 && position < text.Length - 1)
                return new[] {text[position - 1], text[position + 1]};
            return position == 0 ? new[] {text[position + 1]} : new[] {text[position - 1]};
        }
    }
}