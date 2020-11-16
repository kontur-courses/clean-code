using System.Collections.Generic;
using System.Linq;

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
                TokenType.Slash
            };

        public static IEnumerable<Token> Analyze(string text)
        {
            for (var i = 0; i < text.Length; ++i)
            {
                if (!IsToken(text[i].ToString()))
                    continue;

                var slash = text[i].ToString() == TokenType.Slash ? TokenType.Slash : string.Empty;
                var correctTokens = Tokens
                    .Select(token => slash + token)
                    .Where(token => IsTokenContainsInText(token, text, i)).ToArray();
                if (!correctTokens.Any())
                    continue;

                yield return new Token(correctTokens.First(), i);
                i += correctTokens.First().Length - 1;
            }
        }

        private static bool IsToken(string symbol) => Tokens.Contains(symbol);

        private static bool IsTokenContainsInText(string value, string text, int position)
            => !value.Where((letter, i) => i + position >= text.Length || text[i + position] != letter).Any();
    }
}