namespace Markdown;

public static class Tags
{
    public static readonly Tag Italic = new ("Italic", "_", "_", "em");
    public static readonly Tag Bold = new("Bold", "__", "__", "strong");
    public static readonly Tag Header = new("Header", "# ", null, "h1");
    public static readonly Tag Escape = new("Escape", "\\", null, null);
    public static readonly Tag LineFeed = new("Line feed", null, "\n", null);

    public static IEnumerable<Tag> GetAllTags()
    {
        foreach (var fieldInfo in typeof(Tags).GetFields())
        {
            yield return (Tag)fieldInfo.GetValue(typeof(Tags));
        }
    }
}