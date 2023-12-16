namespace Markdown;

public static class TagConverter
{
    private static Dictionary<string, string> symbolsInMd = new()
    {
        { "_", "em" },
        { "__", "strong" },
        { "#", "h1" }
    };

    public static string ConvertMdToHtml(string tag)
    {
        if (symbolsInMd.TryGetValue(tag, out var convertedTag))
            return convertedTag;
        throw new ArgumentException("Unknown symbol");
    }
}