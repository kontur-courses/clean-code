namespace MarkdownRenderer;

public static class StringExtensions
{
    public static string Substring(this string source, Token token)
    {
        return source.Substring(token.Start, token.Length);
    }
}