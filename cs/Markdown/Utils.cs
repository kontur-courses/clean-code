using System;

namespace Markdown
{
    internal static class Utils
    {
        public static bool TryGetSubstring(this string me, int startIndex, int length, out string substr)
        {
            if (startIndex + length <= me.Length)
            {
                substr = me.Substring(startIndex, length);
                return true;
            }
            else
            {
                substr = default;
                return false;
            }
        }

        public static bool IsInsideWordWithNumbers(this string text, int index)
        {
            bool hasDigit(int lookFromIndex, int lookToIndex)
            {
                var di = Math.Sign(lookToIndex - lookFromIndex);
                for (int i = lookFromIndex; i != lookToIndex; i += di)
                {
                    if (char.IsWhiteSpace(text, i))
                        break;
                    if (char.IsDigit(text, i))
                        return true;
                }
                return false;
            }

            return hasDigit(index, 0) || hasDigit(index, text.Length - 1);
        }
    }
}
