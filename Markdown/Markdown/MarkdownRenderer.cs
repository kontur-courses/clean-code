using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Converters;
using Markdown.Parsers;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownRenderer : IMarkdownRenderer
    {
        private readonly IConverter<IEnumerable<MarkdownToken>, IEnumerable<HtmlToken>> converter;
        private readonly IMarkdownParser markdownParser;
        private readonly ITokenizer<MarkdownToken> tokenizer;

        public MarkdownRenderer(
            IMarkdownParser markdownParser,
            ITokenizer<MarkdownToken> tokenizer,
            IConverter<IEnumerable<MarkdownToken>, IEnumerable<HtmlToken>> converter)
        {
            this.converter = converter;
            this.tokenizer = tokenizer;
            this.markdownParser = markdownParser;
        }

        public string Render(string markdown)
        {
            var parsedValues = markdownParser.Parse(markdown);

            var tokens = tokenizer.Tokenize(parsedValues);

            var converted = converter.Convert(tokens);

            return RenderHtml(converted);
        }

        private string RenderHtml(IEnumerable<HtmlToken> tokens)
        {
            var rendered = new StringBuilder();

            foreach (var token in tokens)
            {
                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (token.Type)
                {
                    case TokenType.Word:
                        rendered.Append(token.Value[0] == '\\' && token.Value.Length > 1
                            ? token.Value[1..]
                            : token.Value);
                        break;
                    case TokenType.PairedTagOpened:
                        rendered.Append('<' + token.Value + '>');
                        break;
                    case TokenType.PairedTagClosed:
                        rendered.Append("</" + token.Value + '>');
                        break;
                }
            }

            return rendered.ToString();
        }
    }
}