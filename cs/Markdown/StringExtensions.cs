namespace Markdown
{
    public static class StringExtensions
    {
        public static char? TryGetChar(this string str, int id)
            => id >= 0 && id < str.Length ? str[id] : null as char?;
    }
}