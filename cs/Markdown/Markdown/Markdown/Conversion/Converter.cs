using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Converter : IConverter
    {
        private readonly MarkdownParser mdPraser;
        private readonly MarkdownProcessor mdProcessor;
        private readonly string text;

        public Converter(string text)
        {
            this.text = text;
            mdPraser = new MarkdownParser();
            mdProcessor = new MarkdownProcessor();
        }

        public List<TokenMd> GetTokens()
        {
            return mdPraser.GetTokens(text);
        }

        public List<TokenMd> FormatToken(List<TokenMd> tokens)
        {
            return mdProcessor.FormatTokens(tokens);
        }

        public string GetHTML(List<TokenMd> tokens)
        {
            var builder = new StringBuilder();

            foreach (var token in tokens)
                builder.Append(token.FormattedText);

            return builder.ToString();
        }

        private string GetTokenHtml(TokenMd token)
        {
            var resultToken = token;
            var builder = new StringBuilder();
            
            builder.Append(
                resultToken.InnerTokens == null || resultToken.InnerTokens.Count == 0
                ? resultToken.TokenWithoutMark
                : GetInnerTokensHtml(resultToken.InnerTokens));
            
            return builder.ToString();
        }

        private string GetInnerTokensHtml(List<TokenMd> tokens)
        {
            var builder = new StringBuilder();

            return builder.Append(tokens.Select(GetTokenHtml)).ToString();
        }
    }
}