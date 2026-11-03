namespace Markdown;

internal readonly record struct LinkMatch(string Text, string Url, int End);

internal static class LinkParser
{
    public static bool TryParse(string text, int start, out LinkMatch link)
    {
        throw new NotImplementedException();
    }
}