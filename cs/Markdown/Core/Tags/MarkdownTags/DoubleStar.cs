namespace Markdown.Core.Tags.MarkdownTags
{
    class DoubleStar : IMarkdownTag, IDoubleTag
    {
        public string Opening => "**";
        public string Closing => "**";
    }
}