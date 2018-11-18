using System.Collections.Generic;
using System.Linq;

namespace Markdown.TokenParser
{
    public class MarkdownTokenParser : ITokenParser
    {
        private readonly IEnumerable<string> tags;

        public MarkdownTokenParser(IEnumerable<string> tags)
        {
            this.tags = tags;
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
            if (TryCheckThatPartOfTag(token, nextSymbol, out var nextSymbolIsPartOfTag))
                return nextSymbolIsPartOfTag;
            if (string.IsNullOrWhiteSpace(token))
                return char.IsWhiteSpace(nextSymbol);
            return char.IsLetterOrDigit(nextSymbol);
        }

        private bool TryCheckThatPartOfTag(string token, char nextSymbol, out bool nextSymbolIsPartOfTag)
        {
            nextSymbolIsPartOfTag = false;
            var tokenTags = tags.Where(tag => tag.StartsWith(token));
            var tagVariants = tokenTags as string[] ?? tokenTags.ToArray();
            if (tagVariants.Length <= 0)
                return false;
            nextSymbolIsPartOfTag = tagVariants.Any(tag => tag.StartsWith(token + nextSymbol));
            return true;
        }
    }
}