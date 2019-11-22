using Markdown.MarkdownTags;

namespace Markdown
{
    internal enum TagTokenType
    {
        Opening,
        Closing,
    }

    internal class TagToken
    {
        public readonly MarkdownTagInfo MarkdownTagInfo;
        public readonly TagTokenType TokenType;
        public readonly int Index;

        public TagToken(MarkdownTagInfo markdownTagInfo, int index, TagTokenType tokenType)
        {
            MarkdownTagInfo = markdownTagInfo;
            TokenType = tokenType;
            Index = index;
        }
    }
}
