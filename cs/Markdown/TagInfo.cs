namespace Markdown
{
    public class TagInfo
    {
        public string TagInMd { get; }
        public string OpenTag { get; }
        public string CloseTag { get; }

        public TagInfo(string tagInMd, string openTag, string closeTag)
        {
            TagInMd = tagInMd;
            OpenTag = openTag;
            CloseTag = closeTag;
        }
    }
}