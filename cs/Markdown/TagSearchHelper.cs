namespace Markdown;

internal static class TagSearchHelper
{
    public static bool IsSubstring(string str, string substring, int startIndex)
    {
        return startIndex + substring.Length <= str.Length
            && str.IndexOf(substring, startIndex, substring.Length) == startIndex;
    }

    public static bool IsNewLineStart(string str, int index)
    {
        return index == 0
            || str[index - 1] == '\n';
    }

    public static bool IsWhitespaceOrStringEnd(string str, int index)
    {
        return index == -1
            || index == str.Length
            || char.IsWhiteSpace(str[index]);
    }

    public static bool IsParenthesis(string str, char open, char close, int startIndex, out int length)
    {
        if (startIndex >= 0 && startIndex < str.Length && str[startIndex] == open)
        {
            var nextOpenIndex = str.IndexOf(open, startIndex + 1);
            var closeIndex = nextOpenIndex == -1
                ? str.IndexOf(close, startIndex + 1)
                : str.IndexOf(close, startIndex + 1, nextOpenIndex - startIndex);

            if (closeIndex != -1)
            {
                length = closeIndex - startIndex + 1;
                return true;
            }
        }

        length = default;
        return false;
    }

    public static bool IsNumber(string str, int startIndex, out int length)
    {
        length = 0;

        while (startIndex + length < str.Length && char.IsDigit(str[startIndex + length]))
        {
            length++;
        }

        return length != 0;
    }

    public static bool IsWhiteSpaceSubstring(string str, int startIndex, int length)
    {
        for (var i = startIndex; i < startIndex + length; i++)
        {
            if (!char.IsWhiteSpace(str[i]))
            {
                return false;
            }
        }

        return true;
    }
}
