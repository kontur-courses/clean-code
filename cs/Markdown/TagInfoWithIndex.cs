namespace Markdown
{
    public class TagInfoWithIndex
    {
        public ITagInfo TagInfo { get; }
        public int StartIndex { get; }
        public string TagSubstring { get; }

        public TagInfoWithIndex(ITagInfo tagInfo, int startIndex, string tagSubstring = default)
        {
            TagInfo = tagInfo;
            StartIndex = startIndex;
            TagSubstring = tagSubstring;
        }
    }
}