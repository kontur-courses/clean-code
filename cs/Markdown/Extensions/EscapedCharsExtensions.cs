namespace Markdown.Extensions;

public static class EscapedCharsExtensions
{
    private static readonly char[] EscapedChars = { '\\', '_', '#' };
    private static readonly char[] EscapeChars = { '\\' };

    public static bool IsEscapedBy(this char current, char previous)
    {
        return EscapeChars.Contains(previous) && EscapedChars.Contains(current);
    }
}