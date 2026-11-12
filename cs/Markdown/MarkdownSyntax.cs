namespace Markdown
{
    public static class MarkdownSyntax
    {
        public const char EscapeSymbol = '\\';
        public const char UnderscoreSymbol = '_';

        private static readonly HashSet<char> MarkupSymbolsToEscape = [EscapeSymbol, UnderscoreSymbol];

        private static readonly TagType[] BlockTags = [TagType.Header, TagType.ListItem];

        private static readonly TagType[] UnderscoreTags = [TagType.Emphasis, TagType.Strong];

        private static readonly Dictionary<TagType, string> Markers = new ()
        {
            [TagType.Header] = "# ",
            [TagType.ListItem] = "- ",
            [TagType.Emphasis] = "_",
            [TagType.Strong] = "__",
        };

        public static string GetMarker(TagType tagType)
        {
            return Markers[tagType];
        }

        public static bool CanBeEscaped(char symbol)
        {
            return MarkupSymbolsToEscape.Contains(symbol);
        }

        public static bool TryGetBlockTag(string paragraph, out TagType tagType)
        {
            tagType = BlockTags.FirstOrDefault(tag => paragraph.StartsWith(Markers[tag], StringComparison.Ordinal));
            return tagType != TagType.Unknown;
        }

        public static bool TryGetUnderscoreTag(int underscoreCount, out TagType tagType)
        {
            tagType = UnderscoreTags.FirstOrDefault(tag => Markers[tag].Length == underscoreCount);
            return tagType != TagType.Unknown;
        }
    }
}
