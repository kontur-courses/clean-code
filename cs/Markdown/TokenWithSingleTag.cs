namespace Markdown
{
    public class TokenWithSingleTag : IToken
    {
        public int Length { get; }
        public int StartIndex { get; }
        public ITagInfo TagInfo { get; }
        public int EndTagIndex => StartIndex + Length;

        public TokenWithSingleTag(int startIndex, int length, ITagInfo tagInfo)
        {
            Length = length;
            StartIndex = startIndex;
            TagInfo = tagInfo;
        }
    }
}