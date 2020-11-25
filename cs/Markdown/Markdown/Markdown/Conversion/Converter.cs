using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Converter : IConverter
    {
        private MarkdownParser mdPraser;
        private MarkdownProcessor mdProcessor;
        private string text;

        public Converter(string text)
        {
            this.text = text;
            mdPraser = new MarkdownParser(text);
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

            for (int i = 0; i < tokens.Count; i++)
            {
                var formattedToken = tokens[i].FormattedText;
                builder.Append(formattedToken);
            }

            
            return builder.ToString();
        }

        private string GetTokenHtml(TokenMd token)
        {
            var resultToken = token;
            var builder = new StringBuilder();

            if (resultToken.InnerTokens == null || resultToken.InnerTokens.Count == 0)
                builder.Append(resultToken.TokenWithoutMark);
            else
                builder.Append(GetInnerTokensHtml(resultToken.InnerTokens));
            
            return builder.ToString();
        }

        private string GetInnerTokensHtml(List<TokenMd> tokens)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < tokens.Count; i++)
                builder.Append(GetTokenHtml(tokens[i]));

            return builder.ToString();
        }
    }
}