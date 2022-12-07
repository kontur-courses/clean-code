namespace Markdown
{
    public class HTMLTag : ITag
    {
        public readonly static HTMLTag Strong = new HTMLTag("<strong>", "</strong>");
        public readonly static HTMLTag Emphasys = new HTMLTag("<em>", "</em>");
        public readonly static HTMLTag Heading = new HTMLTag("<h1>", "</h1>");

        public string Opening { get; }
        public string Closing { get; }
        public bool IsSelfClosing => Closing != null;
        public bool CanNesting { get; }

        private HTMLTag(string opening, string closing, bool canNesting = true)
        {
            Opening = opening;
            Closing = closing;
            CanNesting = canNesting;
        }
    }
}
