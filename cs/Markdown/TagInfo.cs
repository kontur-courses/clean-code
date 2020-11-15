namespace Markdown
{
    public class TagInfo
    {
        public string TagInMd { get; }
        public string TagForConverting { get; }
        public bool IsSingle { get; }

        public TagInfo(string tagInMd, string tagForConverting, bool isSingle)
        {
            TagInMd = tagInMd;
            TagForConverting = tagForConverting;
            IsSingle = isSingle;
        }
    }
}