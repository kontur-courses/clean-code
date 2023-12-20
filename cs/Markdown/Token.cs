using Markdown.Tags;

namespace Markdown
{
    public class Token
    {
        private readonly TagType tagType;
        private readonly int startIndex;
        private readonly int endIndex;
        private readonly bool isSingleTag;

        public Token(TagType type, int startIndex, int endIndex, bool isSingleTag = false)
        {
            this.tagType = type;
            this.isSingleTag = isSingleTag;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        public TagType TagType => tagType;
        public int StartIndex => startIndex;
        public int EndIndex => endIndex;
        public bool IsSingleTag => isSingleTag;
    }
}
