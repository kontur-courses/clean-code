namespace Markdown.Core.Tags.MarkdownTags
{
    internal class Apostrophe : IMarkdownTag, IDoubleTag
    {
        public string Opening => "`";
        public string Closing => "`";
    }
}