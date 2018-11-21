namespace Markdown.StringExtension
{
    public static class StringExtension
    {
        public static bool CompareWithSubstring(this string str, int position, string subString)
        {
            var result = false;
            
            if (position + subString.Length < str.Length && position > -1)
                result = str.Substring(position, subString.Length) != subString;
            
            return result;
        }
    }
}