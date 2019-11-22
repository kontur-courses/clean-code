namespace Markdown
{
    public static class StringExtensions
    {
        public static char? TryGetChar(this string str, int id)
            => id < str.Length ? str[id] : null as char?;
    }
}