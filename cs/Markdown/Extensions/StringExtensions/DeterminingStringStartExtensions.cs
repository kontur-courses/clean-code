namespace Markdown.Extensions.StringExtensions
{
    internal static class DeterminingStringStartExtensions
    {
        internal static bool IsEscapeStart(this string text, int index, HashSet<char> markers)
            => text[index] == '\\'
            && index + 1 < text.Length
            && (markers.Contains(text[index + 1])
                || text[index + 1] == '\\'
                || index - 1 >= 0
                    && index + 1 < text.Length
                    && text[index - 1] == '#'
                    && text[index + 1] == ' ');

        internal static bool IsHeaderStart(this string text, int index, string headerMarkerStart)
            => text[index] == headerMarkerStart[0]
            && index + 1 < text.Length
            && text[index + 1] == headerMarkerStart[1] && (index == 0 || text[index - 1] == '\n');

        internal static bool IsBoldStart(this string text, int index, string italicMarkerStart)
            => text.IsItalicStart(index, italicMarkerStart)
            && index + 1 < text.Length && text.IsItalicStart(index + 1, italicMarkerStart);

        internal static bool IsItalicStart(this string text, int index, string italicMarkerStart)
            => text[index] == italicMarkerStart[0];
    }
}
