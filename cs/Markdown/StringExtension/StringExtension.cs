namespace Markdown.StringExtension
{
    public static class StringExtension
    {
        public static bool CompareWithSubstring(this string str, int position, int length, string str2)
        {
            var result = false;
            
            if (position + 1 + str2.Length < str.Length)
                result = str.Substring(position + str2.Length, str2.Length) != str2;
            
            return result;
        }
    }
}