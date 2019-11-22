namespace Markdown
{
    public static class StringExtensions
    {
        public static char? TryGetChar(this string str, int id)
            => id >= 0 && id < str.Length ? str[id] : null as char?;

        public static string GetLeftSubstring(this string str, int index, int length)
        {
            if (index >= str.Length)
                return null;

            return index - length <= 0 ? 
                str.Substring(0, index) : 
                str.Substring(index - length, length);
        }

        public static string GetRightSubstring(this string str, int index, int length)
        {
            if (index >= str.Length || index + 1 == str.Length)
                return null;

            return index + 1 + length >= str.Length ? 
                str.Substring(index + 1, str.Length - index - 1) : 
                str.Substring(index + 1, length);
        }
    }
}