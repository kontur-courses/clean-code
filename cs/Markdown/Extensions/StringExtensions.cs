namespace Markdown.Extensions;

public static class StringExtensions
{
    public static IEnumerable<string> EnumerateLines(this string? markdown)
    {
        if (markdown is null)
        {
            yield break;
        }

        var start = 0;

        for (var i = 0; i < markdown.Length; i++)
        {
            if (markdown[i] != '\n')
            {
                continue;
            }

            yield return markdown[start..i];
            start = i + 1;
        }

        if (start <= markdown.Length)
        {
            yield return markdown[start..];
        }
    }
}