using Markdown.Tags;

namespace Markdown
{
    public class Token
    {
        public Token(Tag type, int startIndex, int endIndex, bool isSingleTag = false)
        {
            TagType = type;
            IsSingleTag = isSingleTag;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public Tag TagType { get; }
        public int StartIndex { get; }
        public int EndIndex { get; }
        public bool IsSingleTag { get; }
    }
}
