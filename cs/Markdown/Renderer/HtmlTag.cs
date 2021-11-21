namespace Markdown
{
    public class HtmlTag
    {
        public readonly string CloseTag;
        public readonly string OpenTag;
        public readonly bool IsPaired;

        public HtmlTag(string openTag, string closeTag, bool isPaired)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
            IsPaired = isPaired;
        }
    }
}