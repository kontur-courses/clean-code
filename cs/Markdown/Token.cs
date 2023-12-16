namespace Markdown
{
    public class Token
    {
        private readonly TagType tagType;
        private readonly int startIndex;
        private readonly int endIndex;
        private readonly bool isSingleTag;

        public Token(TagType type, bool isSingleTag , int startIndex, int endIndex)
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
