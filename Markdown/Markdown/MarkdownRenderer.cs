using System.Collections.Generic;
using Markdown.Converters;
using Markdown.Parsers;
using Markdown.Tokenizers;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        private readonly ITokenizer tokenizer;
        private readonly IMarkdownParser markdownParser;
        private readonly IConverter<IEnumerable<Token>, string> converter;

        public MarkdownRenderer(IMarkdownParser markdownParser,
            ITokenizer tokenizer,
            IConverter<IEnumerable<Token>, string> mdToHtmlConverter)
        {
            this.markdownParser = markdownParser;
            this.tokenizer = tokenizer;
            converter = mdToHtmlConverter;
        }

        public string Render(string markdown)
        {
            var lexemes = markdownParser.ParseMarkdownLexemes(markdown);

            var tokens = tokenizer.Tokenize(lexemes);

            return converter.Convert(tokens);
        }
    }
}