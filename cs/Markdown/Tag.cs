namespace Markdown;

public static class Tag
{
    public static string Open(string tagName) => $"<{tagName}>";

    public static string Close(string tagName) => $"</{tagName}>";

    public static string Wrap(string tagName, string content) => $"{Open(tagName)}{content}{Close(tagName)}";
}