namespace Markdown;

public static class EscapeRules
{
    public const char Character = '\\';

    public static bool IsNotEscaped(string text, int index)
    {
        return !text.IsInBound(index) || !text.IsInBound(index - 1) || text[index - 1] != Character ||
               !IsNotEscaped(text, index - 1);
    }
}