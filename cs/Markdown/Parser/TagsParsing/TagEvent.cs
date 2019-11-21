using Markdown.Parser.Tags;

namespace Markdown.Parser.TagsParsing
{
    public class TagEvent
    {
        public TagEvent(int index, TagEventType type, MarkdownTag tag)
        {
            Index = index;
            Type = type;
            Tag = tag;
        }

        public TagEventType Type { get; }
        public MarkdownTag Tag { get; }
        public int Index { get; }
    }
}