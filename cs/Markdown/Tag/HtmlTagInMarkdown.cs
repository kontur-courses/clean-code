using Markdown.TagRendering;

namespace Markdown.Tag
{
    public class HtmlTagInMarkdown
    {
        public int PositionInMarkdown { get; set; }
        public TagTypes Type { get; set; }
        public TagNames Name { get; set; }
        public string HtmlRepresentation { get; set; }
        public int MarkdownSymbolLength { get; set; }

        public HtmlTagInMarkdown(int positionInMarkdown, TagTypes type, TagNames name, string htmlRepresentation,
            int markdownSymbolLength)
        {
            this.PositionInMarkdown = positionInMarkdown;
            this.Type = type;
            this.Name = name;
            this.HtmlRepresentation = htmlRepresentation;
            this.MarkdownSymbolLength = markdownSymbolLength;
        }
    }
}