namespace Markdown.Core.Tags.MarkdownTags
{
    class Apostrophe : IMarkdownTag, IDoubleTag
    {
        public string Opening => "`";
        public string Closing => "`";
    }
}