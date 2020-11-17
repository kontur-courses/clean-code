namespace Markdown
{
    public class EmphasizingTagInfo : ITagInfo
    {
        public string OpenTagInMd { get; }
        public string TagForConverting { get; }

        public EmphasizingTagInfo(string openTanInMd, string tagForConverting)
        {
            OpenTagInMd = openTanInMd;
            TagForConverting = tagForConverting;
        }
    }
}