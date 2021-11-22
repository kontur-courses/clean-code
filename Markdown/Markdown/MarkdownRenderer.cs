using System;
using Markdown.Converters;
using Markdown.Markings;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        private readonly IConverter<IMarkingTree<MarkdownToken>, IMarkingTree<HtmlToken>> converter;
        private readonly IParser<IMarkingTree<MarkdownToken>> parser;

        public MarkdownRenderer(
            IConverter<IMarkingTree<MarkdownToken>, IMarkingTree<HtmlToken>> converter,
            IParser<IMarkingTree<MarkdownToken>> parser)
        {
            this.converter = converter;
            this.parser = parser;
        }

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}