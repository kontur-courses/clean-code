namespace Markdown
{
    public class TagInfo
    {
        public string TagInMd { get; }
        public string OpenTag { get; }
        public string CloseTag { get; }
        public bool IsSingle { get; } = true;

        public TagInfo(string tagInMd, string openTag)
        {
            TagInMd = tagInMd;
            OpenTag = openTag;
        }

        public TagInfo(string tagInMd, string openTag, string closeTag)
        {
            TagInMd = tagInMd;
            OpenTag = openTag;
            CloseTag = closeTag;
            IsSingle = false;
        }
    }
}