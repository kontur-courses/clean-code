using Markdown.TagClasses;

namespace Markdown
{
    public class MarkdownTagInfo
    {
        public Tag? Tag { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        public bool IsOpening { get; private set; }

        public MarkdownTagInfo(Tag? tag, int startIndex, int endIndex)
        {
            Tag = tag;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public MarkdownTagInfo OpeningVariant()
        {
            IsOpening = true;
            return this;
        }

        public override string ToString()
        {
            return $"{Tag} {StartIndex}";
        }
    }
}
