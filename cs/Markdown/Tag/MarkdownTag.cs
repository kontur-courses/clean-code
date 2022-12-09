namespace Markdown.Tag
{
    public class MarkdownTag : ITag
    {
        public static readonly MarkdownTag Italic = new MarkdownTag("_", "_", false);
        public static readonly MarkdownTag Bold = new MarkdownTag("__", "__");
        public static readonly MarkdownTag Heading = new MarkdownTag("#", "");

        public string Opening { get; }
        public string Closing { get; }
        public bool IsSelfClosing => Closing == string.Empty;
        public bool CanNesting { get; }

        private MarkdownTag(string opening, string closing, bool canNesting = true)
        {
            Opening = opening;
            Closing = closing;
            CanNesting = canNesting;
        }
    }
}
