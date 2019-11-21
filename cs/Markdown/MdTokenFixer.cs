using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdTokenFixer
    {
        //Фильтрует некорректные токены, превращая их в текст
        public List<Token> FixTokens(List<Token> tokens)
        {
            var correctTokens = FilterIncorrectTokens(tokens);
            var fixedTokens = FilterNonMatchingPairTokens(correctTokens);
            return fixedTokens;
        }

        private List<Token> FilterIncorrectTokens(List<Token> tokens)
        {
            var tokenNumber = 0;
            var newTokens = tokens.Select(token =>
            {
                switch (token)
                {
                    case PairToken pairToken:
                        return new PairToken(pairToken.Type, pairToken.Content, pairToken.Length);
                    case HeaderToken headerToken:
                        return new HeaderToken(headerToken.Type, headerToken.Content, headerToken.Length);
                    default:
                        return new Token(token.Type, token.Content, token.Length);
                }
            }).ToList();
            foreach (var token in tokens)
            {
                var prevToken = TryGetTokenByIndex(newTokens, tokenNumber - 1);
                var nextToken = TryGetTokenByIndex(newTokens, tokenNumber + 1);
                newTokens[tokenNumber] = GetCorrectToken(token, prevToken, nextToken);
                tokenNumber++;
            }

            return newTokens;
        }

        private static Token TryGetTokenByIndex(List<Token> tokens, int index)
        {
            if (index >= 0 && index < tokens.Count)
            {
                return tokens[index];
            }

            return null;
        }

        private Token GetCorrectToken(Token tokenToFix, Token prevToken, Token nextToken)
        {
            switch (tokenToFix)
            {
                case PairToken pairToken:
                    return GetCorrectPairToken(pairToken, prevToken, nextToken);
                case HeaderToken headerToken:
                    return GetCorrectHeaderToken(headerToken, prevToken);
                default:
                    return tokenToFix;
            }
        }

        private Token GetCorrectPairToken(PairToken token, Token prevToken, Token nextToken)
        {
            if (IsFirstPairToken(token, prevToken, nextToken))
            {
                return new PairToken(token.Type, token.Content, token.Length, true);
            }

            if (IsSecondPairToken(token, prevToken, nextToken))
            {
                return new PairToken(token.Type, token.Content, token.Length, false);
            }

            return new Token(TokenType.Text, token.Content, token.Length);
        }

        private bool IsFirstPairToken(PairToken token, Token prevToken, Token nextToken)
        {
            var expr = prevToken == null ||
                       prevToken is PairToken pairToken && pairToken.IsFirst ||
                       char.IsWhiteSpace(prevToken.GetLastContentChar()) ||
                       char.IsPunctuation(prevToken.GetLastContentChar()) &&
                       nextToken != null && !char.IsWhiteSpace(nextToken.GetFirstContentChar());
            return expr;
        }

        private bool IsSecondPairToken(PairToken token, Token prevToken, Token nextToken)
        {
            var expr = nextToken == null ||
                       char.IsWhiteSpace(nextToken.GetFirstContentChar()) ||
                       char.IsPunctuation(nextToken.GetFirstContentChar()) ||
                       nextToken.Type != TokenType.Text ||
                       (prevToken is PairToken pairToken && !pairToken.IsFirst) &&
                       prevToken != null && !char.IsWhiteSpace(prevToken.GetLastContentChar());
            return expr;
        }

        private Token GetCorrectHeaderToken(HeaderToken token, Token prevToken)
        {
            if (prevToken == null || prevToken.GetLastContentChar() == '\n')
            {
                return token;
            }

            return new Token(TokenType.Text, token.Content, token.Length);
        }

        private class IndexedToken
        {
            public int Index { get; }
            public Token Token { get; }

            public IndexedToken(int index, Token token)
            {
                Index = index;
                Token = token;
            }
        }

        private List<Token> FilterNonMatchingPairTokens(List<Token> tokens)
        {
            var indexToChangedTokensDict = new Dictionary<int, Token>();
            var indexedTokenStack = new Stack<IndexedToken>();
            var currentTokenIndex = 0;
            foreach (var token in tokens)
            {
                if (token is PairToken pairToken)
                {
                    if (pairToken.IsFirst)
                    {
                        indexedTokenStack.Push(new IndexedToken(currentTokenIndex, pairToken));
                    }
                    else if (indexedTokenStack.Count == 0)
                    {
                        indexToChangedTokensDict[currentTokenIndex] = pairToken.GetTextToken();
                    }
                    else
                    {
                        var prevFirstToken = indexedTokenStack.Peek();
                        if (prevFirstToken.Token.Type == pairToken.Type)
                        {
                            indexedTokenStack.Pop();
                        }
                        else
                        {
                            indexToChangedTokensDict[currentTokenIndex] = pairToken.GetTextToken();
                        }
                    }
                }

                currentTokenIndex++;
            }

            foreach (var token in indexedTokenStack)
            {
                indexToChangedTokensDict[token.Index] = token.Token.GetTextToken();
            }

            var newTokens = new List<Token>();
            var index = 0;
            foreach (var token in tokens)
            {
                newTokens.Add(indexToChangedTokensDict.ContainsKey(index) ? indexToChangedTokensDict[index] : token);
                index++;
            }

            return newTokens;
        }
    }
}