namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly int StartIndex;
        public readonly TagInfo TagInfo;
        public int EndTagIndex => TagInfo.IsSingle ? StartIndex + Length : StartIndex + Length - TagInfo.TagInMd.Length;

        public Token(int startIndex, int length, TagInfo tagInfo)
        {
            StartIndex = startIndex;
            Length = length;
            TagInfo = tagInfo;
        }
    }
}