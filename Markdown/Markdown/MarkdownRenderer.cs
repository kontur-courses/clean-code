using System;
using Markdown.Converters;
using Markdown.Parsers;
using Markdown.Tokens;
using Markdown.Trees;

namespace Markdown
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        private readonly IConverter<IMarkingTree<MarkdownToken>, IMarkingTree<HtmlToken>> converter;
        private readonly IMarkdownParser markdownParser;

        public MarkdownRenderer(
            IConverter<IMarkingTree<MarkdownToken>, IMarkingTree<HtmlToken>> converter,
            IMarkdownParser markdownParser)
        {
            this.converter = converter;
            this.markdownParser = markdownParser;
        }

        public string Render(string markdown)
        {
            throw new NotImplementedException();
        }
    }
}