using System;
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
                var token = currentToken.ToString();
                if (!IsPartOfToken(token, symbol, previousTokenType))
                {
                    var newToken = GetToken(token, previousTokenType);
                    yield return newToken;
                    previousTokenType = newToken.Type;
                    currentToken.Clear();
                }
                currentToken.Append(symbol);
            }
            yield return GetToken(currentToken.ToString(), previousTokenType);
        }

        private bool IsPartOfToken(string token, char nextSymbol, TokenType previousTokenType)
        {
            if (token.Length == 0)
                return true;
            if (token == "\\" || previousTokenType == TokenType.EscapeSymbol)
                return false;
            if (tags.Any(tag => tag.StartsWith(token)))
                return tags.Any(tag => tag.StartsWith(token + nextSymbol));
            if (string.IsNullOrWhiteSpace(token))
                return char.IsWhiteSpace(nextSymbol);
            return char.IsLetterOrDigit(nextSymbol);
        }

        private Token GetToken(string token, TokenType previousTokenType)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token should be not empty string");
            if (previousTokenType == TokenType.EscapeSymbol)
                return new Token(TokenType.Text, token);
            if (token == "\\")
                return new Token(TokenType.EscapeSymbol, token);
            if (tags.Contains(token))
                return new Token(TokenType.Tag, token);
            if (string.IsNullOrWhiteSpace(token))
                return new Token(TokenType.Space, token);
            return new Token(TokenType.Text, token);
        }
    }
}