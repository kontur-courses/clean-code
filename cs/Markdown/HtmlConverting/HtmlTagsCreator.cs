using System.Text;
using Markdown.Tokens;

namespace Markdown.HtmlConverting;

public static class HtmlTagsCreator
{
    private static Dictionary<TagType, string> Tags { get; } = new()
    {
        { TagType.Italic, "em" },
        { TagType.Header, "h1" },
        { TagType.Strong, "strong" },
        { TagType.LinkText, "a" },
    };

    public static string CreateOpenTag(TagType tagType, params (string, string)[] parameters)
    {
        var tagBuilder = new StringBuilder();
        tagBuilder.Append('<');
        tagBuilder.Append(Tags[tagType]);
        
        foreach (var parameter in parameters)
        {
            tagBuilder.Append(' ');
            tagBuilder.Append(parameter.Item1);
            tagBuilder.Append('=');
            tagBuilder.Append('"');
            tagBuilder.Append(parameter.Item2);
            tagBuilder.Append('"');
        }

        tagBuilder.Append('>');
        
        return tagBuilder.ToString();
    } 
    
    public static string CreateCloseTag(TagType tagType) => "</" + Tags[tagType] + ">";
}