namespace Markdown.Core.Tags.MarkdownTags
{
    internal class SingleUnderscore : IMarkdownTag, IDoubleTag
    {
        public string Opening => "_";
        public string Closing => "_";
    }
}