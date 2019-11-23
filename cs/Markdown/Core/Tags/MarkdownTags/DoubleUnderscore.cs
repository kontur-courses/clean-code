namespace Markdown.Core.Tags.MarkdownTags
{
    internal class DoubleUnderscore : IMarkdownTag, IDoubleTag
    {
        public string Opening => "__";
        public string Closing => "__";
    }
}