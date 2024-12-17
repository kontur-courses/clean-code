using Markdown.Html.Tags;

namespace Markdown.Html;

public static class HtmlTagCreator
{
    private static readonly Dictionary<TagType, string> TagToString = new()
    {
        { TagType.Header, "h1" },
        { TagType.Italic, "em" },
        { TagType.Strong, "strong" },
        { TagType.Image, "img" }
    };
    
    public static string GetStringContent(TagType tagType) =>
        TagToString[tagType];
}