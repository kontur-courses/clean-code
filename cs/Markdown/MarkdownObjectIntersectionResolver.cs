using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal static class MarkdownObjectIntersectionResolver
    {
        private static readonly Dictionary<TokenObjectType, List<TokenObjectType>> nestedObjectTypes = new Dictionary<TokenObjectType, List<TokenObjectType>>()
        {
           { TokenObjectType.Strong, new List<TokenObjectType>() { TokenObjectType.Italic } },
           { TokenObjectType.Header, new List<TokenObjectType>() { TokenObjectType.Italic, TokenObjectType.Strong } }
        };

        public static List<Token> ResolveObjectTokenIntersections(List<Token> tokens)
        {
            var objectTokens = tokens.Where(token => token is ObjectToken).ToArray();
            var objectTokenPositions = objectTokens.Select(token => tokens.IndexOf(token)).ToArray();

            var objectStack = new List<(TokenObjectType ObjectType, int OpeningTokenIndex)>();

            int stackOpenTokenIndex;
            int stackCloseTokenIndex;
            TokenObjectType currentStackObjectType;

            for (int currentTokenIndex = 0; currentTokenIndex < objectTokens.Length; currentTokenIndex++)
            {
                var currentToken = objectTokens[currentTokenIndex] as ObjectToken;

                if (!currentToken.IsClose)
                {
                    var canOpened = objectStack.Count < 1 ||
                                    nestedObjectTypes.TryGetValue(objectStack.Last().ObjectType, out var objectTypes) &&
                                    objectTypes.Contains(currentToken.ObjectType);

                    if (canOpened)
                    {
                        objectStack.Add((currentToken.ObjectType, currentTokenIndex));
                        continue;
                    }

                    (currentStackObjectType, stackOpenTokenIndex) = objectStack.Last();

                    var currentCloseTokenIndex = GetCloseObjectTokenIndex(objectTokens,
                                                                          currentTokenIndex,
                                                                          currentToken.ObjectType);

                    tokens[objectTokenPositions[currentTokenIndex]] = Token.EmptyText;
                    tokens[objectTokenPositions[currentCloseTokenIndex]] = Token.EmptyText;

                    stackCloseTokenIndex = GetCloseObjectTokenIndex(objectTokens, currentTokenIndex, currentStackObjectType);
                    if (stackCloseTokenIndex < currentCloseTokenIndex)
                    {
                        tokens[objectTokenPositions[stackOpenTokenIndex]] = Token.EmptyText;
                        tokens[objectTokenPositions[stackCloseTokenIndex]] = Token.EmptyText;
                    }

                    continue;
                }

                (currentStackObjectType, stackOpenTokenIndex) = objectStack.Last();

                if (currentToken.ObjectType == currentStackObjectType)
                {
                    objectStack.Remove((currentStackObjectType, stackOpenTokenIndex));
                    continue;
                }

                var currentOpenTokenIndex = objectStack.FindLastIndex(pair => pair.ObjectType == currentToken.ObjectType);
                if (currentOpenTokenIndex < 0)
                    continue;

                stackCloseTokenIndex = GetCloseObjectTokenIndex(objectTokens, currentTokenIndex, currentStackObjectType);
                if (currentOpenTokenIndex < stackOpenTokenIndex || currentTokenIndex > stackCloseTokenIndex)
                {
                    tokens[objectTokenPositions[stackOpenTokenIndex]] = Token.EmptyText;
                    tokens[objectTokenPositions[stackCloseTokenIndex]] = Token.EmptyText;
                }

                tokens[objectTokenPositions[currentOpenTokenIndex]] = Token.EmptyText;
                tokens[objectTokenPositions[currentTokenIndex]] = Token.EmptyText;

                objectStack.Remove((currentToken.ObjectType, currentOpenTokenIndex));
            }

            return tokens;
        }

        private static int GetCloseObjectTokenIndex(Token[] tokens, int start, TokenObjectType type)
        {
            return Array.FindIndex(tokens, start, token => token is ObjectToken &&
                                                           (token as ObjectToken)!.IsClose &&
                                                           (token as ObjectToken)!.ObjectType == type);
        }
    }
}