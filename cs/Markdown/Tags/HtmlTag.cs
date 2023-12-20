namespace Markdown.Tags
{
    public class HtmlTag
    {
        private readonly TagType type;
        private readonly int index;
        private readonly bool isClosing;
        private readonly string markup;

        public HtmlTag(TagType type, int index, bool isClosing, string htmlMarkup)
        {
            this.type = type;
            this.index = index;
            this.isClosing = isClosing;
            markup = htmlMarkup;
        }

        public TagType Type => type;
        public int Index => index;
        public bool IsClosing => isClosing;

        public string GetMarkup()
        {
            if (type == TagType.EscapedSymbol)
                return "";
            return IsClosing ? string.Format("</{0}>", markup) : string.Format("<{0}>", markup);
        }
    }
}
