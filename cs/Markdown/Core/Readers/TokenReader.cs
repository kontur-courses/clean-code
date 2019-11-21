using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core.Readers
{
    public class TokenReader
    {
        private const char EscapeSymbol = '\\';
        private static readonly HashSet<char> SpaceSymbols = new HashSet<char>() {' ', '\n'};

        public List<IToken> ReadTokens(string source)
        {
            var tokens = new List<IToken>();
            var escapedPositions = new List<int>();
            var isHeaderPossible = true;
            var wordBuilder = new StringBuilder();
            for (var i = 0; i < source.Length; i++)
            {
                var currentChar = source[i];
                var currentPosition = i - escapedPositions.Count;
                if (currentChar == EscapeSymbol && !escapedPositions.Contains(currentPosition))
                {
                    escapedPositions.Add(currentPosition);
                    continue;
                }

                var isEnding = i == source.Length - 1;
                if (SpaceSymbols.Contains(currentChar) || isEnding)
                {
                    if (isEnding)
                    {
                        wordBuilder.Append(currentChar);
                        currentPosition += 1;
                    }

                    var word = wordBuilder.ToString();
                    var wordPosition = currentPosition - word.Length;

                    if (isHeaderPossible)
                    {
                        var headerToken = ReadHeaderToken(word, wordPosition, escapedPositions);
                        if (headerToken != null)
                        {
                            tokens.Add(headerToken);
                            wordPosition += headerToken.Length;
                            word = word.Substring(headerToken.Length);
                        }

                        isHeaderPossible = false;
                    }

                    var (openingToken, textToken, closingToken) = ReadTokensFromWord(
                        word, wordPosition, escapedPositions);
                    if (openingToken != null)
                        tokens.Add(openingToken);
                    tokens.Add(textToken);
                    if (closingToken != null)
                        tokens.Add(closingToken);
                    if (!isEnding)
                        tokens.Add(new SpaceToken(currentPosition, currentChar.ToString()));
                    wordBuilder.Clear();
                    continue;
                }

                wordBuilder.Append(currentChar);
            }

            return tokens;
        }

        private Token ReadHeaderToken(string word, int wordPosition, List<int> escapedPositions)
        {
            foreach (var headerTag in TagsUtils.MdBeginningTags)
            {
                if (IsValidPositionForOpeningTag(word, headerTag, wordPosition, escapedPositions))
                    return new HTMLTagToken(wordPosition, headerTag, HTMLTagType.Header);
            }

            return null;
        }

        private (Token OpeningToken, Token TextToken, Token ClosingToken) ReadTokensFromWord(
            string word, int wordPosition, List<int> escapedPositions)
        {
            Token openingToken = null;
            Token closingToken = null;
            foreach (var inlineTag in TagsUtils.MdInlineTags)
            {
                if (openingToken == null
                    && IsValidPositionForOpeningTag(word, inlineTag, wordPosition, escapedPositions))
                {
                    openingToken = new HTMLTagToken(wordPosition, inlineTag, HTMLTagType.Opening);
                }

                var closingTokenPossiblePosition = wordPosition + word.Length - inlineTag.Length;
                if (closingToken == null
                    && IsValidPositionForClosingTag(word, inlineTag, closingTokenPossiblePosition, escapedPositions))
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

        private bool IsValidPositionForOpeningTag(string word, string tag,
            int wordPosition, List<int> escapedPositions)
        {
            return word.StartsWith(tag)
                   && !Enumerable.Range(wordPosition, tag.Length).Any(escapedPositions.Contains);
        }

        private bool IsValidPositionForClosingTag(string word, string inlineTag,
            int closingTokenPossiblePosition, List<int> escapedPositions)
        {
            return word.EndsWith(inlineTag)
                   && !Enumerable.Range(closingTokenPossiblePosition, inlineTag.Length).Any(escapedPositions.Contains);
        }
    }
}