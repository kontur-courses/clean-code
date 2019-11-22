namespace Markdown
{
    internal enum TagTokenType
    {
        Opening,
        Closing,
    }

    internal class TagToken
    {
        public readonly MarkdownTag MarkdownTag;
        public readonly TagTokenType TokenType;
        public readonly int Index;

        public TagToken(MarkdownTag markdownTag, int index, TagTokenType tokenType)
        {
            MarkdownTag = markdownTag;
            TokenType = tokenType;
            Index = index;
        }
    }
}
