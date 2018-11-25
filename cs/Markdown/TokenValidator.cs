using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenValidator
    {
        public IEnumerable<Paragraph> SeparateByParagraphs(IEnumerable<SingleToken> tokens, string mdText)
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

        public void FillParagraphWithHtmlTags(IEnumerable<Paragraph> paragraphs)
        {
            foreach (var paragraph in paragraphs)
            {
                FillParagraphWithHtmlTagsOfInlineTokens(paragraph);
                FillParagraphWithHtmlTagsOfStartingTokens(paragraph);
            }
        }

        public void FillParagraphWithHtmlTagsOfStartingTokens(Paragraph paragraph)
        {
            paragraph.HtmlTags.AddRange(GetHtmlTagsOfStartingTokens(paragraph.StartingTokens, paragraph.End, paragraph.Start));
        }

        public void FillParagraphWithHtmlTagsOfInlineTokens(Paragraph paragraph)
        {
            paragraph.HtmlTags.AddRange(GetHtmlTagsOfInlineTokens(paragraph.InlineTokens));
        }

        public IEnumerable<HtmlTag> GetHtmlTagsOfStartingTokens(IEnumerable<SingleToken> tokens, int paragraphEnd, int paragraphStart)
        {
            var validHtmlTags = new List<HtmlTag>();
            var firstToken = tokens.FirstOrDefault();
            if (firstToken != null)
            {
                validHtmlTags.Add(new HtmlTag(firstToken.TokenType.HtmlTag, firstToken.TokenPosition, firstToken.LocationType));
                validHtmlTags.Add(new HtmlTag(firstToken.TokenType.HtmlTag, paragraphEnd - paragraphStart, firstToken.LocationType));
            }
            else
            {
                validHtmlTags.Add(new HtmlTag("p", 0, LocationType.Opening));
                validHtmlTags.Add(new HtmlTag("p", paragraphEnd - paragraphStart, LocationType.Closing));
            }

            return validHtmlTags;
        }

        public IEnumerable<HtmlTag> GetHtmlTagsOfInlineTokens(IEnumerable<SingleToken> tokens)
        {
            var validHtmlTags = new List<HtmlTag>();
            var notClosedTokens = new List<SingleToken>();

            foreach (var token in tokens)
            {
                if (token.LocationType == LocationType.Opening)
                {
                    notClosedTokens.Add(token);
                }
                else if (token.LocationType == LocationType.Closing)
                {
                    var lastIndex = notClosedTokens.Select(t => t.TokenType)
                        .ToList()
                        .LastIndexOf(token.TokenType);
                    if (lastIndex < 0)
                        continue;

                    validHtmlTags.Add(new HtmlTag(token.TokenType.HtmlTag, token.TokenPosition, LocationType.Closing));
                    validHtmlTags.Add(new HtmlTag(notClosedTokens[lastIndex].TokenType.HtmlTag, notClosedTokens[lastIndex].TokenPosition, LocationType.Opening));
                    notClosedTokens.RemoveRange(lastIndex, notClosedTokens.Count - lastIndex);
                }
                else
                {
                    throw new InvalidOperationException("Invalid location type");
                }
            }

            return validHtmlTags;
        }

        public IEnumerable<SingleToken> ValidStartingTokens()
        {
            throw new NotImplementedException();
        }
    }
}
