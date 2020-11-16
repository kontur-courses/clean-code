namespace Markdown
{
    public class TagInfo
    {
        public string TagInMd { get; }
        public string TagForConverting { get; }
        public bool IsSingle { get; }
        public string TagEndSym { get; }

        public TagInfo(string tagInMd, string tagForConverting, bool isSingle, string tagEndSym = "")
        {
            TagInMd = tagInMd;
            TagForConverting = tagForConverting;
            IsSingle = isSingle;
            TagEndSym = tagEndSym;
        }

        public TagInfo(string tagInMd, string tagForConverting)
        {
            TagInMd = tagInMd;
            TagForConverting = tagForConverting;
            IsSingle = false;
            TagEndSym = null;
        }
    }
}