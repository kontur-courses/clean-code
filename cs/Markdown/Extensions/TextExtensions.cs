namespace Markdown.Extensions;

internal static class TextExtensions
{
    internal static bool IsInBounds(this string text, int index, int length)
    {
        ArgumentNullException.ThrowIfNull(text);

        return index >= 0 && index + length <= text.Length;
    }

    internal static bool HasFollowingSpace(this string text, int index)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (index < 0 || index >= text.Length)
        {
            return false;
        }

        return char.IsWhiteSpace(text[index]);
    }

    internal static bool ContainsTagAtIndex(this string text, int index, string tagValue)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (index < 0 || index + tagValue.Length > text.Length)
        {
            return false;
        }

        return text.Substring(index, tagValue.Length).Equals(tagValue);
    }

    internal static bool HasAdjacentTag(this string text, int index, string tagValue)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (index < 0 || index + tagValue.Length > text.Length)
        {
            return false;
        }

        return text.Substring(index, tagValue.Length).Equals(tagValue);
    }
}