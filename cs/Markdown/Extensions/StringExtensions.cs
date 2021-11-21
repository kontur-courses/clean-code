namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool InRange(this string str, int position)
        {
            return position < str.Length && position >= 0;
        }

        public static int LastIndex(this string str)
        {
            return str.Length - 1;
        }

        public static bool HasCharsBehind(this string str, int position, int amount)
        {
            return str.InRange(position - amount);
        }

        public static bool TryGetCharsBehind(this string str, int position, int amount, out char[] chars)
        {
            if (str.InRange(position - amount))
            {
                chars = str.Substring(position - amount, amount).ToCharArray();
                return true;
            }
            chars = default;
            return false;
        }

        // можно объединить
        public static bool TryGetNextChars(this string str, int position, int amount, out char[] chars)
        {
            if (str.InRange(position + amount))
            {
                chars = str.Substring(position + amount, amount).ToCharArray();
                return true;
            }
            chars = default;
            return false;
        }

        public static bool TrySubstring(this string str, int position, int length, out string substring)
        {
            if (str.InRange(position + length - 1))
            {
                substring = str.Substring(position, length);
                return true;
            }
            substring = default;
            return false;
        }
    }
}
