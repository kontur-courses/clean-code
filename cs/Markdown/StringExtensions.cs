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

        public static int IndexOf(this string originalString, char value, int startIndex)
        {
            var res = -1;
            for (int i = startIndex; i < originalString.Length; i++)
                if (originalString[i] == value)
                {
                    res = i;
                    break;
                }
            return res;
        }
    }
}