namespace Markdown.HtmlTag
{
    public class HtmlTag
    {
        public readonly string StartTag;
        public readonly string EndTag;

        public HtmlTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException("Tag must be with arguments");
            StartTag = $@"\<{tag}>";
            EndTag = $@"\</{tag}>";
        }
        public HtmlTag(string startTag, string endTag)
        {
            if (string.IsNullOrEmpty(startTag) || string.IsNullOrEmpty(endTag))
                throw new ArgumentNullException("Tag must be with arguments");
            StartTag = startTag;
            EndTag = endTag;
        }
    }
}
