namespace Markdown
{
    public class ParagraphRenderer
    {
        private static readonly BlockTag[] BlockTags = [BlockTag.Header, BlockTag.ListItem];

        private readonly InlineRenderer inlineRenderer = new ();

        public string Render(string paragraph)
        {
            if (IsBlockTagEscaped(paragraph))
                return inlineRenderer.Render(paragraph.Substring(1));

            if (TryFindBlockTag(paragraph, out var blockTag))
            {
                var content = paragraph.Substring(blockTag.Prefix.Length);
                return HtmlTag.Wrap(blockTag.TagName, inlineRenderer.Render(content));
            }

            return inlineRenderer.Render(paragraph);
        }

        public bool IsListItem(string paragraph)
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
            return startsWithEscape && TryFindBlockTag(paragraph.Substring(1), out _);
        }
    }
}
