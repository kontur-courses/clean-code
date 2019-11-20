namespace Markdown.Core.Tags.MarkdownTags
{
    class Star : IMarkdownTag, IDoubleTag
    {
        public string Opening => "*";
        public string Closing => "*";
    }
}