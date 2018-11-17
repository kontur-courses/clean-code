using System.Collections.Generic;
using System.Linq;

namespace Markdown.TokenParser
{
    public class MarkdownTokenParser : ITokenParser
    {
        private readonly HashSet<string> tags;

        public MarkdownTokenParser(IEnumerable<string> tags)
        {
            this.tags = new HashSet<string>(tags.OrderByDescending(tag => tag.Length));
        }

        public IEnumerable<string> GetTokens(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;
            var currentToken = new Queue<char>();
            foreach (var symbol in text)
            {
                var token = new string(currentToken.ToArray());
                if (!IsPartOfToken(token, symbol))
                {
                    yield return token;
                    currentToken.Clear();
                }
                currentToken.Enqueue(symbol);
            }
            yield return new string(currentToken.ToArray());
        }

        private bool IsPartOfToken(string token, char nextSymbol)
        {
            if (token.Length == 0)
                return true;
            if (token == "\\" || nextSymbol == '\\' || token == "\n" || nextSymbol == '\n')
                return false;
            var tokenTag = tags.FirstOrDefault(tag => tag.StartsWith(token));
            if (tokenTag != null)
                return tokenTag.StartsWith(token + nextSymbol);
            if (string.IsNullOrWhiteSpace(token))
                return char.IsWhiteSpace(nextSymbol);
            return char.IsLetterOrDigit(nextSymbol);
        }
    }
}