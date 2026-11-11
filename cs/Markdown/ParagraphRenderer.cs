namespace Markdown
{
    public static class ParagraphRenderer
    {
        private static readonly BlockTag[] BlockTags = [BlockTag.Header, BlockTag.ListItem];

        public static string Render(string paragraph)
        {
            if (IsBlockTagEscaped(paragraph))
                return InlineRenderer.Render(paragraph[1..]);

            if (TryFindBlockTag(paragraph, out var blockTag))
            {
                var content = paragraph[blockTag.Prefix.Length..];
                return HtmlTag.Wrap(blockTag.TagName, InlineRenderer.Render(content));
            }

            return InlineRenderer.Render(paragraph);
        }

        public static bool IsListItem(string paragraph)
        {
            return TryFindBlockTag(paragraph, out var blockTag) && blockTag == BlockTag.ListItem;
        }

        private static bool TryFindBlockTag(string paragraph, out BlockTag blockTag)
        {
            blockTag = BlockTags.FirstOrDefault(tag => paragraph.StartsWith(tag.Prefix, StringComparison.Ordinal));
            return blockTag != null;
        }

        private static bool IsBlockTagEscaped(string paragraph)
        {
            var startsWithEscape = paragraph.StartsWith(EscapeFinder.EscapeSymbol);
            return startsWithEscape && TryFindBlockTag(paragraph[1..], out _);
        }
    }
}
