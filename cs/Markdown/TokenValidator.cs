using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenValidator
    {
        public IEnumerable<Paragraph> GetParagraphsWithValidTokens(IEnumerable<SingleToken> tokens, string mdText)
        {
            var paragraphs = SeparateByParagraphs(tokens, mdText);
            FillParagraphWithValidTokens(paragraphs);

            return paragraphs;
        }

        private IEnumerable<Paragraph> SeparateByParagraphs(IEnumerable<SingleToken> tokens, string mdText)
        {
            var paragraphs = new List<Paragraph>();

            var paragraphStart = 0;
            var inlineTokens = new List<SingleToken>();
            var startingTokens = new List<SingleToken>();

            foreach (var token in tokens)
            {
                if (token.TokenType.TokenLocationType == TokenLocationType.EndLineToken)
                {
                    var paragraphEnd = token.TokenPosition;
                    paragraphs.Add(new Paragraph(
                        paragraphStart,
                        paragraphEnd,
                        mdText.Substring(paragraphStart, paragraphEnd - paragraphStart),
                        inlineTokens,
                        startingTokens));

                    inlineTokens = new List<SingleToken>();
                    startingTokens = new List<SingleToken>();
                    paragraphStart = paragraphEnd + 1;
                }
                if (token.TokenType.TokenLocationType == TokenLocationType.StartingToken)
                    startingTokens.Add(new SingleToken(token.TokenType, token.TokenPosition - paragraphStart, token.LocationType));
                if (token.TokenType.TokenLocationType == TokenLocationType.InlineToken)
                    inlineTokens.Add(new SingleToken(token.TokenType, token.TokenPosition - paragraphStart, token.LocationType));
            }
            if (mdText.Length > paragraphStart)
            {
                var paragraphEnd = mdText.Length;
                paragraphs.Add(new Paragraph(
                    paragraphStart,
                    paragraphEnd,
                    mdText.Substring(paragraphStart, paragraphEnd - paragraphStart),
                    inlineTokens,
                    startingTokens));
            }

            return paragraphs;
        }

        private void FillParagraphWithValidTokens(IEnumerable<Paragraph> paragraphs)
        {
            foreach (var paragraph in paragraphs)
            {
                paragraph.ValidTokens.AddRange(GetValidStartingTokens(paragraph.StartingTokens, paragraph.End, paragraph.Start));
                paragraph.ValidTokens.AddRange(GetValidInlineTokens(paragraph.InlineTokens));
            }
        }

        private IEnumerable<SingleToken> GetValidStartingTokens(IEnumerable<SingleToken> tokens, int paragraphEnd, int paragraphStart)
        {
            var validHtmlTags = new List<SingleToken>();
            var firstToken = tokens.FirstOrDefault();
            if (firstToken != null)
            {
                validHtmlTags.Add(firstToken);
                validHtmlTags.Add(new SingleToken(firstToken.TokenType, paragraphEnd - paragraphStart, LocationType.Closing));
            }
            else
            {
                var paragraphTokenType = new TokenType(TokenTypeEnum.Paragraph, "", "p", TokenLocationType.StartingToken);
                validHtmlTags.Add(new SingleToken(paragraphTokenType, 0, LocationType.Opening));
                validHtmlTags.Add(new SingleToken(paragraphTokenType, paragraphEnd - paragraphStart, LocationType.Closing));
            }

            return validHtmlTags;
        }

        private IEnumerable<SingleToken> GetValidInlineTokens(IEnumerable<SingleToken> tokens)
        {
            var validTokens = new List<SingleToken>();
            var notClosedTokens = new List<SingleToken>();

            foreach (var token in tokens)
            {
                if (token.LocationType == LocationType.Opening)
                {
                    notClosedTokens.Add(token);
                }
                else if (token.LocationType == LocationType.Closing)
                {
                    var lastIndex = notClosedTokens
                        .Where(t => t.TokenPosition !=token.TokenPosition)
                        .Select(t => t.TokenType)
                        .ToList()
                        .LastIndexOf(token.TokenType);
                    if (lastIndex < 0)
                        continue;

                    validTokens.Add(token);
                    validTokens.Add(notClosedTokens[lastIndex]);
                    notClosedTokens.RemoveRange(lastIndex, notClosedTokens.Count - lastIndex);
                }
                else
                {
                    throw new InvalidOperationException("Invalid location type");
                }
            }

            return validTokens;
        }
    }
}
