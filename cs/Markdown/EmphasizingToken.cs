namespace Markdown
{
    public class EmphasizingToken : IToken
    {
        public int Length { get; }
        public int StartIndex { get; }
        public ITagInfo TagInfo { get; }
        public int EndTagIndex => StartIndex + Length - TagInfo.OpenTagInMd.Length;

        public EmphasizingToken(int startIndex, int length, ITagInfo tagInfo)
        {
            StartIndex = startIndex;
            Length = length;
            TagInfo = tagInfo;
        }
    }
}