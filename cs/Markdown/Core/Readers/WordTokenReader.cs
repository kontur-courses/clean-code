using System.Collections.Generic;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core.Readers
{
    public class WordTokenReader
    {
        public (Token OpeningToken, Token TextToken, Token ClosingToken) ReadTokensFromWord(
            string word, int wordPosition, List<int> escapedPositions)
        {
            Token openingToken = null;
            Token closingToken = null;
            foreach (var inlineTag in TagsUtils.MdInlineTags)
            {
                if (openingToken == null
                    && ReaderUtils.IsValidPositionForOpeningTag(word, inlineTag, wordPosition, escapedPositions))
                {
                    openingToken = new HTMLTagToken(wordPosition, inlineTag, HTMLTagType.Opening);
                }

                var closingTokenPossiblePosition = wordPosition + word.Length - inlineTag.Length;
                if (closingToken == null
                    && ReaderUtils.IsValidPositionForClosingTag(word, inlineTag, closingTokenPossiblePosition,
                        escapedPositions))
                {
                    closingToken = new HTMLTagToken(closingTokenPossiblePosition, inlineTag, HTMLTagType.Closing);
                }

                if (openingToken != null && closingToken != null)
                    break;
            }

            var textToken = GetTextTokenByOpeningAndClosingToken(word, wordPosition, openingToken, closingToken);

            return (openingToken, textToken, closingToken);
        }

        private TextToken GetTextTokenByOpeningAndClosingToken(string word, int wordPosition,
            Token openingToken, Token closingToken)
        {
            var textPosition = wordPosition;
            var textLength = word.Length;
            if (openingToken != null)
            {
                textPosition += openingToken.Value.Length;
                textLength -= openingToken.Value.Length;
            }

            if (closingToken != null)
            {
                textLength -= closingToken.Value.Length;
            }

            var textToken = new TextToken(textPosition, word.Substring(textPosition - wordPosition, textLength));
            return textToken;
        }
    }
}