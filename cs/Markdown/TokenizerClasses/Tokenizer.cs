using System.Collections.Generic;
using System.Linq;
using Markdown.TokenizerClasses.Scanners;

namespace Markdown.TokenizerClasses
{
    public class Tokenizer
    {
        private readonly List<IScanner> tokenScanners = new List<IScanner>
        {
            new TagScanner(),
            new PlainTextScanner()
        };

        public IEnumerable<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            Token token;

            do
            {
                token = GetNextToken(text);
                if (tokens.Count > 0)
                {
                    var previousToken = tokens.Last();

                    if (CheckDoubleUnderscore(token, previousToken, out var newToken)
                        || CheckConsecutiveNumbers(token, previousToken, out newToken)
                        || CheckEscapedTokens(token, previousToken, out newToken))
                    {
                        token = newToken;
                        tokens.RemoveLast();
                    }
                }
                tokens.Add(token);

                if (token.Length > text.Length)
                    break;

                text = text.Substring(token.Length, text.Length - token.Length);
            } while (token.Type != TokenType.EOF);

            RemoveEOFToken(tokens);

            return tokens;
        }

        public Token GetNextToken(string text)
        {
            foreach (var scanner in tokenScanners)
                if (scanner.TryScan(text, out var token))
                    return token;

            return Token.EOF;
        }

        private static void RemoveEOFToken(List<Token> tokens) => tokens.RemoveLast();

        private bool CheckDoubleUnderscore(Token token, Token previousToken, out Token newToken)
        {
            if (token.Type == TokenType.Underscore && previousToken.Type == TokenType.Underscore)
            {
                newToken = new Token(TokenType.DoubleUnderscore, "__");
                return true;
            }

            newToken = token;
            return false;
        }

        private bool CheckConsecutiveNumbers(Token token, Token previousToken, out Token newToken)
        {
            if (token.Type == TokenType.Num && previousToken.Type == TokenType.Num)
            {
                newToken = new Token(TokenType.Num, previousToken.Value + token.Value);
                return true;
            }

            newToken = token;
            return false;
        }

        private bool CheckEscapedTokens(Token token, Token previousToken, out Token newToken)
        {
            if ((token.Type == TokenType.Underscore
                || token.Type == TokenType.DoubleUnderscore
                || token.Type == TokenType.EscapeChar)
                && previousToken.Type == TokenType.EscapeChar)
            {
                newToken = new Token(TokenType.Text, token.Value);
                return true;
            }

            newToken = token;
            return false;
        }
    }

    public static class TokenizeExtensions
    {
        public static void RemoveLast(this List<Token> tokens)
        {
            if (tokens.Count > 0)
                tokens.RemoveAt(tokens.Count - 1);
        }
    }
}