namespace MarkdownRenderer;

public static class StringExtensions
{
    public static bool TryReadToken(this string source,
        int start,
        Func<string, int, bool> endCondition,
        out Token? token)
    {
        for (var i = start + 1; i < source.Length; i++)
        {
            if (!endCondition(source, i))
                continue;
            token = new Token(start, i);
            return true;
        }

        token = null;
        return false;
    }

    public static string Substring(this string source, Token token)
    {
        return source.Substring(token.Start, token.Length);
    }
}