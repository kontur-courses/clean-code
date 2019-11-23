namespace Markdown.Core.Tags.MarkdownTags
{
    internal class Star : IMarkdownTag, IDoubleTag
    {
        public string Opening => "*";
        public string Closing => "*";
    }
}