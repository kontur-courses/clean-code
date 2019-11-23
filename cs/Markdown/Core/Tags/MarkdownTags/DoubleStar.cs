namespace Markdown.Core.Tags.MarkdownTags
{
    internal class DoubleStar : IMarkdownTag, IDoubleTag
    {
        public string Opening => "**";
        public string Closing => "**";
    }
}