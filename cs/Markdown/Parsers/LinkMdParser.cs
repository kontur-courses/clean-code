namespace Markdown.Parsers
{
    public class LinkMdParser : BaseParser
    {
        public static readonly Tag Tag = MdTags.Link;
        
        public LinkMdParser() : base(Tag)
        {
        } 
        
        public override Token TryParseTag(int position, string text)
        {
            if (!HasThisTagOpening(position, text)) return null;
            var probablyLinkPosition = IndexOfLink(position, text);
            if (probablyLinkPosition == -1) return null;
            if (probablyLinkPosition == position + 3) return null;
            var endOfLink = text.IndexOf(')', probablyLinkPosition);
            return endOfLink == -1 || endOfLink == probablyLinkPosition + 1
                ? null
                : new LinkToken(position, endOfLink - position + 1,
                    MdTags.Link, TextType.Link, probablyLinkPosition,
                    text.Substring(probablyLinkPosition, endOfLink - probablyLinkPosition));
        }

        private static int IndexOfLink(int position, string text)
        {
            var endText = text.IndexOf(']', position);
            if (endText == -1) return -1;
            if (text.Length == endText + 1) return -1;
            if (text[endText + 1] != '(') return -1;
            if (text.Length <= endText + 3) return -1;
            return endText + 2;
        }
    }
}