namespace Markdown
{
    public static class StringExtenstion
    {
        public static bool InBorders(this string str, int position)
        {
            var length = str.Length;
            return position >= 0 && position < length;
        }
    }
}