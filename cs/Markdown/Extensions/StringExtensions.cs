namespace Markdown.Extensions;

public static class StringExtensions
{
    public static bool IsEscaped(this string text, int index)
    {
        if (index < 0 || text.Length <= index)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        return index != 0 && text[index - 1] == '\\' && !IsEscaped(text, index - 1);
    }
}