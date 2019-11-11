namespace Markdown
{
    internal static class Utils
    {
        public static bool TryGetSubstring(this string me, int startIndex, int length, out string substr)
        {
            if (startIndex + length <= me.Length)
            {
                substr = me.Substring(startIndex, length);
                return true;
            }
            else
            {
                substr = default;
                return false;
            }
        }

        public static bool IsInsideWordWithNumbers(this string text, int index)
        {
            return false;
        }
    }
}
