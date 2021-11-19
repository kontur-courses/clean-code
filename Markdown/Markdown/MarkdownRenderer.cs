using System;
using Markdown.Converters;
using Markdown.Markings;
using Markdown.Parsers;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        private readonly IConverter<IMarking<MarkdownToken>, IMarking<HtmlToken>> converter;
        private readonly IParser<MarkdownMarking> parser;

        public MarkdownRenderer(
            IConverter<IMarking<MarkdownToken>, IMarking<HtmlToken>> converter,
            IParser<MarkdownMarking> parser)
        {
            this.converter = converter;
            this.parser = parser;
        }

        public string Render(string markdown)
        {
            var parsedMarking = parser.Parse(markdown);

            var htmlMarking = converter.Convert(parsedMarking);

            return htmlMarking.ToString();
        }
    }
}