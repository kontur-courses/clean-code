namespace Markdown.Renderer
{
    public class HtmlTag
    {
        public readonly string CloseTag;
        public readonly bool IsPaired;
        public readonly string OpenTag;

        public HtmlTag(string openTag, string closeTag, bool isPaired)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
            IsPaired = isPaired;
        }
    }
}