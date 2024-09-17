namespace Markdown.Extensions;

public static class CharExtensions
{
    private static readonly char[] escapedChars = { '\\', '_', '#' };
    private static readonly char[] escapeChars = { '\\' };

    public static bool IsEscapedBy(this char current, char previous)
    {
        return escapeChars.Contains(previous) && escapedChars.Contains(current);
    }
}