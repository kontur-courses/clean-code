namespace Markdown
{
    public static class StringExtensions
    {
        public static bool StartsWith(this string originalString, string value, int startIndex)
        {
            for (int i = 0; i < value.Length; i++)
            {
                var indexForOriginalString = startIndex + i;
                if (indexForOriginalString >= originalString.Length ||
                        originalString[indexForOriginalString] != value[i])
                    return false;
            }
            return true;
        }
    }
}