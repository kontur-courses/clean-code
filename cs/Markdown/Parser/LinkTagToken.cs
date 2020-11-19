namespace Markdown
{
    public class LinkTagToken : PairedTagToken
    {
        public readonly string LinkText;
        public readonly string LinkUrl;

        public LinkTagToken(int startPosition, int endPosition, TagType type, string linkText, string linkUrl) : base(startPosition, endPosition, type)
        {
            LinkText = linkText;
            LinkUrl = linkUrl;
        }

        public override int TagSignLength => EndPosition - StartPosition;

        public override int GetReplacedValueLength(bool isCloser)
        {
            return isCloser ? 1 : TagSignLength;
        }

        public override string GetHtmlValue(bool isCloser)
        {
            return isCloser ? "" : $"<a href={LinkUrl}>{LinkText}</a>";
        }
    }
}