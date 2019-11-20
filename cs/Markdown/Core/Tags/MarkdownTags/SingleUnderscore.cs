namespace Markdown.Core.Tags.MarkdownTags
{
    class SingleUnderscore : IMarkdownTag, IDoubleTag
    {
        public string Opening => "_";
        public string Closing => "_";
    }
}