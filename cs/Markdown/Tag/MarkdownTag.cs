namespace Markdown
{
    public class MarkdownTag : ITag
    {
        public readonly static MarkdownTag Italic = new MarkdownTag("_", "_", false);
        public readonly static MarkdownTag Bold = new MarkdownTag("__", "__");
        public readonly static MarkdownTag Heading = new MarkdownTag("#", "");

        public string Opening { get; }
        public string Closing { get; }
        public bool IsSelfClosing => Closing == string.Empty;
        public bool CanNesting { get; }

        public MarkdownTag(string opening, string closing, bool canNesting = true)
        {
            Opening = opening;
            Closing = closing;
            CanNesting = canNesting;
        }
    }
}
