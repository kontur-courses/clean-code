using System.Collections.Generic;

namespace Markdown.TokenParser
{
    public class MarkdownTokenParser : ITokenParser
    {
        private readonly HashSet<string> tags;

        public MarkdownTokenParser(IEnumerable<string> tags)
        {
            this.tags = new HashSet<string>(tags);
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
            if (tags.Contains(token))
                return tags.Contains(token + nextSymbol);
            if (string.IsNullOrWhiteSpace(token))
                return char.IsWhiteSpace(nextSymbol);
            return char.IsLetterOrDigit(nextSymbol);
        }
    }
}