using System;
using System.Collections.Generic;
using System.Linq;
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
            var currentToken = new Queue<char>();
            var previousTokenType = TokenType.ParagraphStart;
            foreach (var symbol in text)
            {
                var token = new string(currentToken.ToArray());
                if (!IsPartOfToken(token, symbol))
                {
                    var newToken = GetToken(token, previousTokenType);
                    yield return newToken;
                    previousTokenType = newToken.Type;
                    currentToken.Clear();
                }
                currentToken.Enqueue(symbol);
            }
            yield return GetToken(new string(currentToken.ToArray()), previousTokenType);
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