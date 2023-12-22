namespace Markdown.Extensions;

public static class StringExtension
{
    public static bool IsTagStart(this string content, Dictionary<string, TagType> tagDictionary)
    {
        return tagDictionary.Any(tag => tag.Key.StartsWith(content));
    }

    public static bool IsTag(this string content, Dictionary<string, TagType> tagDictionary)
    {
        return tagDictionary.Any(tag => tag.Key == content);
    }

    public static bool IsTagSequenceEnd(this string currentContent, string currentChar, Dictionary<string, TagType> tagDictionary)
    {
        return IsTagStart(currentContent + currentChar, tagDictionary) || IsTag(currentContent + currentChar, tagDictionary);
    }
}