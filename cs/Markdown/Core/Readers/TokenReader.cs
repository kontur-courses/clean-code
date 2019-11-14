using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.HTMLTags;
using Markdown.Core.Tokens;

namespace Markdown.Core.Readers
{
    public class TokenReader
    {
        private const char EscapeSymbol = '\\';
        private static readonly List<string> TagNames = new List<string>() {"__", "_"};
        private static readonly HashSet<char> SpaceSymbols = new HashSet<char>() {' ', '\n'};

        public List<IToken> ReadTokens(string source)
        {
            var tokens = new List<IToken>();
            var escapedPositions = new List<int>();
            var word = new StringBuilder();
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
                        word.Append(currentChar);
                        currentPosition += 1;
                    }
                    var (openingToken, textToken, closingToken) = ReadTokensFromWord(
                        word.ToString(), currentPosition - word.Length, escapedPositions);
                    if (openingToken != null)
                        tokens.Add(openingToken);
                    tokens.Add(textToken);
                    if (closingToken != null)
                        tokens.Add(closingToken);
                    if (!isEnding)
                        tokens.Add(new SpaceToken(currentPosition, currentChar.ToString()));
                    word.Clear();
                    continue;
                }
                word.Append(currentChar);
            }
            return tokens;
        }

        private (Token OpeningToken, Token TextToken, Token ClosingToken) ReadTokensFromWord(
            string word, int startIndex, List<int> escapedPositions)
        {
            Token openingToken = null;
            Token closingToken = null;
            foreach (var tagName in TagNames)
            {
                if (openingToken == null
                    && IsValidPositionForOpeningTag(word, tagName, startIndex, escapedPositions))
                {
                    openingToken = new HTMLTagToken(startIndex, tagName, true);
                }

                var closingTokenPossiblePosition = startIndex + word.Length - tagName.Length;
                if (closingToken == null 
                    && IsValidPositionForClosingTag(word, tagName, closingTokenPossiblePosition, escapedPositions))
                {
                    closingToken = new HTMLTagToken(closingTokenPossiblePosition, tagName, false);
                }

                if (openingToken != null && closingToken != null)
                    break;
            }
            
            var textToken = GetTextTokenByOpeningAndClosingToken(word, startIndex, openingToken, closingToken);

            return (openingToken, textToken, closingToken);
        }

        private TextToken GetTextTokenByOpeningAndClosingToken(string value, int startIndex, 
            Token openingToken, Token closingToken)
        {
            var textPosition = startIndex;
            var textLength = value.Length;
            if (openingToken != null)
            {
                textPosition += openingToken.Value.Length;
                textLength -= openingToken.Value.Length;
            }

            if (closingToken != null)
            {
                textLength -= closingToken.Value.Length;
            }

            var textToken = new TextToken(textPosition, value.Substring(textPosition - startIndex, textLength));
            return textToken;
        }

        private bool IsValidPositionForOpeningTag(string value, string tagName,
            int startIndex, List<int> escapedPositions)
        {
            return value.StartsWith(tagName)
                   && !Enumerable.Range(startIndex, tagName.Length).Any(escapedPositions.Contains);
        }

        private bool IsValidPositionForClosingTag(string value, string tagName,
            int closingTokenPossiblePosition, List<int> escapedPositions)
        {
            return value.EndsWith(tagName)
                && !Enumerable.Range(closingTokenPossiblePosition, tagName.Length).Any(escapedPositions.Contains);
        }
    }
}