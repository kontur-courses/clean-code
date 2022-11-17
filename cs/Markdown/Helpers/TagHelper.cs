namespace Markdown.Helpers;

public static class TagHelper
{
    public static List<T> GetAllTags<T>()
    {
        throw new NotImplementedException();
    }

    public static (string start, string end) GetHtmlFormat(string tagName) => ($"<{tagName}>", $"</{tagName}>");
}