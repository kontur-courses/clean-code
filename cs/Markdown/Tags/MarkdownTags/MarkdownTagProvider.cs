namespace Markdown.Tags.MarkdownTags;

public static class MarkdownTagProvider
{
    public static readonly Tag Bold = new MarkdownBoldTag();
    public static readonly Tag Italics = new MarkdownItalicsTag();
    public static readonly Tag Heading = new MarkdownHeadingTag();
    
    public static readonly List<Tag> SupportedTags;

    static MarkdownTagProvider()
    {
        SupportedTags = new List<Tag>();
        foreach (var field in typeof(MarkdownTagProvider).GetFields())
        {
            if (field.FieldType == typeof(Tag))
                SupportedTags.Add((Tag)field.GetValue(null)!);
        }
    }
}