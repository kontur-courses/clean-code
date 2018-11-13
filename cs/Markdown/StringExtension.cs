namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsEscapedCharAt(this string str, int index)
        {
            return index == 0 || str[index - 1] == '\\';
        }
    }
}
