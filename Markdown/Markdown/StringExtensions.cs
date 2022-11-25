namespace Markdown;

public static class StringExtensions
{
    public static bool IsSubstringAt(this string substring, string target, int index)
    {
        return target.IsRangeInBound(index, substring.Length)
               && !substring.Where((c, i) => target[index + i] != c).Any();
    }

    public static bool IsInBound(this string str, int index)
    {
        return index >= 0 && index < str.Length;
    }

    public static bool IsRangeInBound(this string str, int start, int length)
    {
        return length > 0 && str.IsInBound(start) && str.IsInBound(start + length - 1);
    }

    public static bool IsRangeNotBoundedWith(this string text, int position, int length, char character)
    {
        return (!text.IsInBound(position - 1) || text[position - 1] != character)
               && (!text.IsInBound(position + length) || text[position + length] != character);
    }

    public static bool IsRangeAtStartOfWord(this string text, int position, int length)
    {
        return position == 0 || (text[position - 1] == ' '
                                 && text.IsInBound(position + 1)
                                 && text[position + 1] != ' ');
    }

    public static bool IsRangeAtEndOfWord(this string text, int position, int length)
    {
        var positionAfterRange = position + length;
        return positionAfterRange >= text.Length || (text[positionAfterRange] == ' '
                                                     && text[position - 1] != ' ');
    }

    public static bool IsInNumericWord(this string text, int inWordPosition)
    {
        return char.IsDigit(text[inWordPosition])
               || HasNumbersBeforeSpace(text, inWordPosition, -1)
               || HasNumbersBeforeSpace(text, inWordPosition, 1);
    }

    public static bool HasNumbersBeforeSpace(this string text, int index, int increment)
    {
        for (var i = index; text.IsInBound(i); i += increment)
        {
            if (char.IsDigit(text[i]))
                return true;
            if (text[i] == ' ')
                break;
        }

        return false;
    }

    public static bool HasInRange(this string text, int firstPosition, int lastPosition, char character)
    {
        for (var i = firstPosition; i <= lastPosition; i++)
            if (text[i] == character)
                return true;

        return false;
    }
}