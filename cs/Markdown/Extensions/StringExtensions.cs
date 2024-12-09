namespace Markdown.Extensions;

public static class StringExtensions
{
    public static string[] SplitIntoLines(this string text)
    {
        return text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    public static bool NextCharIs(this string line, char ch, int currentIndex)
    {
        return currentIndex + 1 < line.Length && line[currentIndex + 1] == ch;
    }
}