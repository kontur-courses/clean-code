namespace Markdown
{
    public static class StringExtension
    {
        public static bool IsEscapedCharAt(this string str, int index)
        {
            if (index == 0 || str[index - 1] != '\\')
                return false;
            return !IsEscapedCharAt(str, index - 1);
        }
    }
}
