namespace Markdown.Tag
{
    public class MarkdownTag
    {
        public int PositionInMarkdown { get; }
        public TagTypes Type { get; }
        public TagNames Name { get; }
        public string HtmlRepresentation { get; }
        public int MarkdownSymbolLength { get; }

        public MarkdownTag(int positionInMarkdown, TagTypes type, TagNames name, string htmlRepresentation,
            int markdownSymbolLength)
        {
            PositionInMarkdown = positionInMarkdown;
            Type = type;
            Name = name;
            HtmlRepresentation = htmlRepresentation;
            MarkdownSymbolLength = markdownSymbolLength;
        }
    }
}