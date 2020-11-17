namespace Markdown
{
    public class SingleTagInfo : ITagInfo
    {
        public string OpenTagInMd { get; }
        public string TagForConverting { get; }
        public string TagEndSym { get; }

        public SingleTagInfo(string openTanInMd, string tagForConverting, string tagEndSym)
        {
            OpenTagInMd = openTanInMd;
            TagForConverting = tagForConverting;
            TagEndSym = tagEndSym;
        }
    }
}