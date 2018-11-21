using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Data;

namespace Markdown.TokenParser
{
    public class MarkdownTokenParser : ITokenParser
    {
        private readonly IEnumerable<string> tags;

        public MarkdownTokenParser(IEnumerable<string> tags)
        {
            this.tags = tags;
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            if (string.IsNullOrEmpty(text))
                yield break;
            var currentToken = new StringBuilder();
            var previousTokenType = TokenType.ParagraphStart;
            foreach (var symbol in text)
            {
                var tokenText = currentToken.ToString();
                if (!IsPartOfToken(tokenText, symbol, previousTokenType))
                {
                    var token = GetToken(tokenText, previousTokenType);
                    yield return token;
                    previousTokenType = token.Type;
                    currentToken.Clear();
                }
                currentToken.Append(symbol);
            }
            yield return GetToken(currentToken.ToString(), previousTokenType);
        }

        private bool IsPartOfToken(string tokenText, char nextSymbol, TokenType previousTokenType)
        {
            if (tokenText.Length == 0)
                return true;
            if (tokenText == "\\" || previousTokenType == TokenType.EscapeSymbol)
                return false;
            if (tags.Any(tag => tag.StartsWith(tokenText)))
                return tags.Any(tag => tag.StartsWith(tokenText + nextSymbol));
            if (string.IsNullOrWhiteSpace(tokenText))
                return char.IsWhiteSpace(nextSymbol);
            return char.IsLetterOrDigit(nextSymbol);
        }

        private Token GetToken(string tokenText, TokenType previousTokenType)
        {
            if (previousTokenType == TokenType.EscapeSymbol)
                return new Token(TokenType.Text, tokenText);
            if (tokenText == "\\")
                return new Token(TokenType.EscapeSymbol, tokenText);
            if (tags.Contains(tokenText))
                return new Token(TokenType.Tag, tokenText);
            if (string.IsNullOrWhiteSpace(tokenText))
                return new Token(TokenType.Space, tokenText);
            return new Token(TokenType.Text, tokenText);
        }
    }
}