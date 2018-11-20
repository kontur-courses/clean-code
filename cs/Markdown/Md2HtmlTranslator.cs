using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Md2HtmlTranslator
    {
        public string TranslateMdToHtml(string mdText, IEnumerable<SingleToken> tokens)
        {
            var htmlText = GetHtmlText(mdText, tokens.OrderBy(token => token.TokenPosition));

            return htmlText;
        }

        private string GetOpeningHtmlTagsForStartingTokens(IEnumerable<SingleToken> startingTokens)
        {
            var htmlBuilder = new StringBuilder();
            foreach (var token in startingTokens)
            {
                htmlBuilder.Append($"<{token.TokenType.HtmlTag}>");
            }

            return htmlBuilder.ToString();
        }

        private string GetClosingHtmlTagsForStartingTokens(IEnumerable<SingleToken> startingTokens)
        {
            var htmlBuilder = new StringBuilder();
            foreach (var token in startingTokens.Reverse())
            {
                htmlBuilder.Append($"<{token.TokenType.HtmlTag}>");
            }

            return htmlBuilder.ToString();
        }

        private string GetHtmlTagsForInlineTokens(string mdText, IEnumerable<SingleToken> inlineTokens, int index)
        {
            var htmlBuilder = new StringBuilder();
            foreach (var token in inlineTokens)
            {
                var htmlTag = token.LocationType == LocationType.Opening ? $"<{token.TokenType.HtmlTag}>" :
                        token.LocationType == LocationType.Closing ? $"</{token.TokenType.HtmlTag}>" :
                        throw new InvalidOperationException("Invalid token location type");

                htmlBuilder.Append(mdText.Substring(index, token.TokenPosition - index));
                htmlBuilder.Append(htmlTag);

                index = token.TokenPosition + token.TokenType.Template.Length;
            }

            return htmlBuilder.ToString();
        }

        private int GetParagraphStartIndex(SingleToken lastStartingToken)
        {
            if (lastStartingToken == null)
                return 0;
            return lastStartingToken.TokenPosition + lastStartingToken.TokenType.Template.Length + 1;
        }

        private (List<SingleToken> inlineTokens, List<SingleToken> startingTokens)
            GetSeparatedTokensByLocationTypes(IEnumerable<SingleToken> tokensStream)
        {
            var inlineTokens = new List<SingleToken>();
            var startingTokens = new List<SingleToken>();
            foreach (var token in tokensStream)
            {
                if (token.TokenType.TokenLocationType == TokenLocationType.StartingToken)
                    startingTokens.Add(token);
                else if (token.TokenType.TokenLocationType == TokenLocationType.InlineToken)
                    inlineTokens.Add(token);
                else
                    throw new InvalidOperationException("Invalid token location type");
            }

            return (inlineTokens, startingTokens);
        }

        private string GetHtmlText(string mdText, IEnumerable<SingleToken> tokensStream)
        {
            var separatedTokens = GetSeparatedTokensByLocationTypes(tokensStream);
            var startingTokens = separatedTokens.startingTokens;
            var inlineTokens = separatedTokens.inlineTokens;

            var htmlBuilder = new StringBuilder();

            htmlBuilder.Append(GetOpeningHtmlTagsForStartingTokens(startingTokens));
            var startIndex = GetParagraphStartIndex(startingTokens.LastOrDefault());
            var htmlTextWithInlineHtmlTags = GetHtmlTagsForInlineTokens(mdText, inlineTokens, startIndex);
            htmlBuilder.Append(htmlTextWithInlineHtmlTags);
            htmlBuilder.Append(GetClosingHtmlTagsForStartingTokens(startingTokens));

            return htmlBuilder.ToString();
        }
    }
}
