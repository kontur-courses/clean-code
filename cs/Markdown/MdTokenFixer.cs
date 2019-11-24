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
            var newTokens = tokens.Select(token => token.Clone()).ToList();
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
                    return ConvertHeaderToTextIfNotFirst(headerToken, prevToken);
                default:
                    return tokenToFix;
            }
        }

        private Token GetCorrectPairToken(PairToken token, Token prevToken, Token nextToken)
        {
            if (IsFirstPairToken(prevToken, nextToken))
            {
                return new PairToken(token.Type, token.Content, token.Length, true);
            }

            if (IsSecondPairToken(prevToken, nextToken))
            {
                return new PairToken(token.Type, token.Content, token.Length, false);
            }

            return new Token(TokenType.Text, token.Content, token.Length);
        }

        private bool IsFirstPairToken(Token prevToken, Token nextToken)
        {
            return prevToken == null ||
                   PrevSymbolIsCorrectToFirstPair(prevToken) &&
                   NextSymbolIsNotWhitespace(nextToken);
        }

        private bool PrevSymbolIsCorrectToFirstPair(Token prevToken)
        {
            var prevSymbol = prevToken.GetLastContentChar();
            return prevToken is PairToken pairToken && pairToken.IsFirst ||
                   char.IsWhiteSpace(prevSymbol) ||
                   char.IsPunctuation(prevSymbol);
        }

        private bool NextSymbolIsNotWhitespace(Token nextToken)
        {
            return nextToken != null && !char.IsWhiteSpace(nextToken.GetFirstContentChar());
        }

        private bool IsSecondPairToken(Token prevToken, Token nextToken)
        {
            return (nextToken == null || NextSymbolIsCorrectToSecondPair(nextToken)) &&
                   PrevSymbolIsNotWhitespace(prevToken);
        }

        private bool NextSymbolIsCorrectToSecondPair(Token nextToken)
        {
            var nextSymbol = nextToken.GetFirstContentChar();
            return char.IsWhiteSpace(nextSymbol) ||
                   char.IsPunctuation(nextSymbol) ||
                   nextToken.Type != TokenType.Text;
        }

        private bool PrevSymbolIsNotWhitespace(Token prevToken)
        {
            return prevToken != null && !char.IsWhiteSpace(prevToken.GetLastContentChar());
        }

        private Token ConvertHeaderToTextIfNotFirst(HeaderToken token, Token prevToken)
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
                if (!(token is PairToken pairToken))
                {
                    currentTokenIndex++;
                    continue;
                }

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