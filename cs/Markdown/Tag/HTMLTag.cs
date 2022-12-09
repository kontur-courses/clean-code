namespace Markdown.Tag
{
    public class HtmlTag : ITag
    {
        public static readonly HtmlTag Strong = new HtmlTag("<strong>", "</strong>");
        public static readonly HtmlTag Emphasis = new HtmlTag("<em>", "</em>");
        public static readonly HtmlTag Heading = new HtmlTag("<h1>", "</h1>");

        public string Opening { get; }
        public string Closing { get; }
        public bool IsSelfClosing => Closing != null;
        public bool CanNesting { get; }

        private HtmlTag(string opening, string closing, bool canNesting = true)
        {
            Opening = opening;
            Closing = closing;
            CanNesting = canNesting;
        }
    }
}
