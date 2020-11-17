namespace Markdown
{
    public class LinkTagInfo : IAttributeTagInfo
    {
        public string OpenTagInMd { get; }
        public string TagForConverting { get; }

        public char[] TagSymbols { get; } = {'(', ')', ']'};

        public LinkTagInfo(string openTagInMd, string tagForConverting)
        {
            OpenTagInMd = openTagInMd;
            TagForConverting = tagForConverting;
        }
    }
}