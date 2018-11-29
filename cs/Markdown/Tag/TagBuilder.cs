using System;
using Markdown.TagRendering;

namespace Markdown.Tag
{
    public class TagBuilder
    {
        private int positionInMarkdown = 0;
        private TagTypes type = TagTypes.Opening;
        private TagNames name = TagNames.Em;
        private string markdownRepresentation = "";
        private int markdownSymbolLength = 0;

        public static TagBuilder Tag()
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
            if (string.IsNullOrEmpty(representation))
                throw new ArgumentException("Representation of a tag can't be null or empty");
            this.markdownRepresentation = representation;
            return this;
        }

        public TagBuilder WithMarkdownSymbolLength(int symbolLength)
        {
            if (symbolLength < 0)
                throw new ArgumentException("The symbols length can't be negative");
            this.markdownSymbolLength = symbolLength;
            return this;
        }

        public MarkdownTag Build() => new MarkdownTag(positionInMarkdown, type, name, markdownRepresentation,
            markdownSymbolLength);
    }
}