namespace Markdown.Core.Tags.MarkdownTags
{
    class DoubleUnderscore : IMarkdownTag, IDoubleTag
    {
        public string Opening => "__";
        public string Closing => "__";
    }
}