

namespace Markdown;

internal static class TagToHtml
{
    public static string GetTagAction(TypeTagAction tagAction, char ch)
    {
        return tagAction == TypeTagAction.Open ? OpenTag[ch] : CloseTag[ch];
    }
    private static readonly Dictionary<char, string> OpenTag = new()
    {
        {'_', "<em>" },
        {';', "<strong>" },
        {'#',  "<h1>" }
    };
        
    private static readonly Dictionary<char, string> CloseTag = new()
    {
        {'_', @"</em>" },
        {';', @"</strong>" },
        {'#',  @"</h1>" }
    };
}