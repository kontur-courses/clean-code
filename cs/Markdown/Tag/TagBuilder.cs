using Markdown.TagRendering;

namespace Markdown.Tag
{
    public class TagBuilder
    {
        private const int PositionInMarkdownDefault = 0;
        private const TagTypes TypeDefault = TagTypes.Opening;
        private const TagNames NameDefault = TagNames.Em;
        private const string MarkdownRepresentationDefault = "";
        private const int MarkdownSymbolLengthDefault = 0;


        private int positionInMarkdown = PositionInMarkdownDefault;
        private TagTypes type = TypeDefault;
        private TagNames name = NameDefault;
        private string markdownRepresentation = MarkdownRepresentationDefault;
        private int markdownSymbolLength = MarkdownSymbolLengthDefault;

        public static TagBuilder ATag()
        {
            return new TagBuilder();
        }

        public TagBuilder InPosition(int position)
        {
            this.positionInMarkdown = position;
            return this;
        }

        public TagBuilder WithType(TagTypes tagType)
        {
            this.type = tagType;
            return this;
        }

        public TagBuilder WithName(TagNames tagName)
        {
            this.name = tagName;
            return this;
        }

        public TagBuilder WithHtmlRepresentation(string representation)
        {
            this.markdownRepresentation = representation;
            return this;
        }

        public TagBuilder WithMarkdownSymbolLength(int symbolLength)
        {
            this.markdownSymbolLength = symbolLength;
            return this;
        }

        public HtmlTagInMarkdown Build() => new HtmlTagInMarkdown(positionInMarkdown, type, name, markdownRepresentation,
            markdownSymbolLength);
    }
}