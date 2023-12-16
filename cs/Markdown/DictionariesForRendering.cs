

namespace Markdown.ConvertToHtml;

internal class ConverterToHtml
{
    public static string ChangeTagInMarkdown(TypeActionMarkdown action, char x)
    {
        return action == TypeActionMarkdown.Open ? openTag[x] : closeTag[x];
    }
    private static Dictionary<char, string> openTag = new()
    {
        {';', "<strong>" },
        {'_', "<em>" },
        {'#',  "<h1>" }
    };
        
    private static Dictionary<char, string> closeTag = new()
    {
        {';', @"\<strong>" },
        {'_', @"<\em>" },
        {'#',  @"<\h1>" }
    };
}