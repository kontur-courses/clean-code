using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Lexer
    {
        public static readonly HashSet<string> Tokens
            = new HashSet<string>
            {
                TokenType.Bold,
                TokenType.Italic,
                TokenType.Heading,
                TokenType.Slash,
                TokenType.SquareBracket
            };

        public static readonly List<string> PartsOfLink
            = new List<string>
            {
                TokenType.SquareBracket,
                TokenType.BackSquareBracket,
                TokenType.RoundBracket,
                TokenType.BackRoundBracket
            };

        public static IEnumerable<Token> Analyze(string text)
        {
            for (var i = 0; i < text.Length; ++i)
            {
                if (!IsToken(text[i].ToString()))
                    continue;
                if (text[i].ToString() == TokenType.SquareBracket)
                {
                    var partsOfLink = GetPartsOfLink(text, i);
                    if (partsOfLink != null)
                    {
                        var token = new Token(partsOfLink.ToArray(), i);
                        yield return token;
                        i += token.Text.Length - 1;
                    }
                    continue;
                }
                    
                var slash = text[i].ToString() == TokenType.Slash ? TokenType.Slash : string.Empty;
                var correctTokens = Tokens
                    .Select(token => slash + token)
                    .Concat(PartsOfLink.Select(partOfLink => slash + partOfLink))
                    .Where(token => IsTokenContainsInText(token, text, i)).ToArray();
                if (!correctTokens.Any())
                    continue;

                yield return new Token(correctTokens.Take(1).ToArray(), i);
                i += correctTokens.First().Length - 1;
            }
        }

        private static List<string> GetPartsOfLink(string text, int i)
        {
            var resultList = new List<string>();
            var tokenValue = new StringBuilder();
            var position = 0;
            var j = i;
            for (; j < text.Length; ++j)
            {
                var symbol = text[j].ToString();
                if (PartsOfLink.Where(x => x != PartsOfLink[position]).Contains(symbol))
                    return null;
                if (symbol != PartsOfLink[position])
                {
                    tokenValue.Append(symbol);
                    continue;
                }
                if (j - 1 >= 0 && text[j - 1].ToString() == TokenType.Slash)
                    return null;
                if (tokenValue.Length > 0)
                    resultList.Add(tokenValue.ToString());
                resultList.Add(symbol);

                tokenValue.Clear();
                if (++position == PartsOfLink.Count)
                    break;
            }
            return IsCorrectLink(resultList, text.Substring(i + 1, j - i)) ? resultList : null;
        }

        private static bool IsCorrectLink(List<string> resultList, string linkText)
        {
            var roundBracketPosition = linkText.IndexOf(TokenType.RoundBracket, StringComparison.Ordinal);
            return linkText[roundBracketPosition - 1].ToString() == TokenType.BackSquareBracket
                   && !resultList[4].Contains(" ");
        }

        private static bool IsToken(string symbol) => Tokens.Contains(symbol);

        private static bool IsTokenContainsInText(string value, string text, int position)
            => !value.Where((letter, i) => i + position >= text.Length || text[i + position] != letter).Any();
    }
}