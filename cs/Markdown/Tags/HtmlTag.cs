namespace Markdown.Tags
{
    public class HtmlTag
    {
        public HtmlTag(Tag tag, int index, bool isClosing, string htmlMarkup)
        {
            Tag = tag;
            Index = index;
            IsClosing = isClosing;
            Markup = htmlMarkup;
        }

        public Tag Tag { get; }
        public int Index { get; }
        public bool IsClosing { get; }
        public string Markup { get; }

        public string GetMarkup()
        {
            if (Tag == Tag.EscapedSymbol)
            {
                return "";
            }

            return IsClosing ? string.Format("</{0}>", Markup) : string.Format("<{0}>", Markup);
        }
    }
}
