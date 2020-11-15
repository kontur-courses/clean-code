namespace Markdown
{
    public class TagInfoWithIndex
    {
        public TagInfo TagInfo { get; }
        public int StartIndex { get; }

        public TagInfoWithIndex(TagInfo tagInfo, int startIndex)
        {
            TagInfo = tagInfo;
            StartIndex = startIndex;
        }
    }
}