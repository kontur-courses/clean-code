namespace Markdown.Tag
{
    public class HtmlTag
    {
        public TagNames TagName { get; set; }
        public OpeningAndClosingTagPair<string, string> OpeningAndClosingTagPair { get; set; }
        public string MarkdownStringOrSymbol { get; set; }
    }
}