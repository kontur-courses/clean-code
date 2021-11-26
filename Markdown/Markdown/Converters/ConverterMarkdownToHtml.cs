using System.Collections.Generic;
using Markdown.Factories;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public class ConverterMarkdownToHtml : IConverter<IEnumerable<MarkdownToken>, IEnumerable<HtmlToken>>
    {
        private readonly ITokenFactory<HtmlToken> tokenFactory;

        public ConverterMarkdownToHtml(ITokenFactory<HtmlToken> tokenFactory)
        {
            this.tokenFactory = tokenFactory;
        }

        public IEnumerable<HtmlToken> Convert(IEnumerable<MarkdownToken> markdownTokens)
        {
            var htmlTokens = new List<HtmlToken>();
            foreach (var token in markdownTokens)
            {
                if (token.Type != TokenType.Word)
                {
                    switch (token.Value)
                    {
                        case "\n":
                            htmlTokens.Add(tokenFactory.NewToken(token.Type, "h1"));
                            htmlTokens.Add(tokenFactory.NewToken(TokenType.Word, "\n"));
                            break;
                        case "# " or "#" or "":
                            htmlTokens.Add(tokenFactory.NewToken(token.Type, "h1"));
                            break;
                        case "_":
                            htmlTokens.Add(tokenFactory.NewToken(token.Type, "em"));
                            break;
                        case "__":
                            htmlTokens.Add(tokenFactory.NewToken(token.Type, "strong"));
                            break;
                    }
                }
                else
                {
                    htmlTokens.Add(tokenFactory.NewToken(TokenType.Word, token.Value));
                }
            }

            return htmlTokens;
        }
    }
}