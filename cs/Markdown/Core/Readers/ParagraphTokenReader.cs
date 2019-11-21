using System.Collections.Generic;
using System.Text;
using Markdown.Core.Tokens;

namespace Markdown.Core.Readers
{
    public class ParagraphTokenReader
    {
        private const char EscapeSymbol = '\\';
        private static readonly HashSet<char> SpaceSymbols = new HashSet<char>() {' ', '\n'};
        private readonly HeaderReader headerReader = new HeaderReader();
        private readonly WordTokenReader wordTokenReader = new WordTokenReader();

        public List<Token> ReadTokens(string source)
        {
            var tokens = new List<Token>();
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
                        var headerToken = headerReader.ReadHeaderToken(word, wordPosition, escapedPositions);
                        if (headerToken != null)
                        {
                            tokens.Add(headerToken);
                            wordPosition += headerToken.Length;
                            word = word.Substring(headerToken.Length);
                        }

                        isHeaderPossible = false;
                    }

                    var (openingToken, textToken, closingToken) = wordTokenReader.ReadTokensFromWord(
                        word, wordPosition, escapedPositions);
                    if (openingToken != null)
                        tokens.Add(openingToken);
                    if (textToken.Value != "")
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
    }
}