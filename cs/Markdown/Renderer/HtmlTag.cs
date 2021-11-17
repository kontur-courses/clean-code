namespace Markdown
{
    public class HtmlTag
    {
        public readonly string CloseTag;
        public readonly string OpenTag;

        public HtmlTag(string openTag, string closeTag)
        {
            OpenTag = openTag;
            CloseTag = closeTag;
        }
    }
}