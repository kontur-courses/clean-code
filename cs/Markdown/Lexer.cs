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
                TokenType.Slash
            };

        public static bool IsToken(string symbol) => Tokens.Contains(symbol);

        public static Token Analyze(StringBuilder text, int position)
        {
            var slash = text[position].ToString() == TokenType.Slash ? TokenType.Slash : string.Empty;
            foreach (var token in Tokens.Where(token => IsTokenContainsInText(slash + token, text, position)))
                return new Token(slash + token, position);
            return new Token(text[position].ToString(), position);
        }

        private static bool IsTokenContainsInText(string value, StringBuilder text, int position)
            => !value.Where((letter, i) => i + position >= text.Length || text[i + position] != letter).Any();
    }
}