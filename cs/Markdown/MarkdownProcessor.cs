using System.Collections.Generic;
using System.Linq;
using Markdown.Html;
using Markdown.Tokens;

namespace Markdown
{
    public class MarkdownProcessor
    {
        private readonly IHtmlConverter htmlConverter;

        public MarkdownProcessor(IHtmlConverter htmlConverter)
        {
            this.htmlConverter = htmlConverter;
        }

        public string RenderToHtml(string markdownText)
        {
            var tokens = GetAllTokens(markdownText);
            var htmlString = ReplaceTokensToHtmlInText(markdownText, tokens);
            return ReplaceEscapeSymbols(htmlString);
        }

        private IEnumerable<Token> GetAllTokens(string text)
        {
            return new TokenReader(text).ReadAllTokens(new MarkdownSeparatorHandler());
        }

        private string ReplaceTokensToHtmlInText(string text, IEnumerable<Token> tokens)
        {
            var htmlText = text;
            foreach (var token in tokens.Where(t => t is TwoSeparatorToken).Cast<TwoSeparatorToken>())
            {
                var tokenValueInHtml =
                    htmlConverter.ConvertSeparatedStringToPairedHtmlTag(token.TokenValue, token.Separator);
                htmlText = htmlText.Replace(token.TokenValueWithSeparators, tokenValueInHtml);
            }

            return htmlText;
        }

        private string ReplaceEscapeSymbols(string text)
        {
            return text.Replace("\\_", "_");
        }
    }
}