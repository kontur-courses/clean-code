using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    internal static class MarkdownTokenAnalyzer
    {
        private static readonly Dictionary<TokenObjectType, List<TokenObjectType>> nestedObjectTypes = new Dictionary<TokenObjectType, List<TokenObjectType>>()
        {
           { TokenObjectType.Strong, new List<TokenObjectType>() { TokenObjectType.Italic } },
           { TokenObjectType.Header, new List<TokenObjectType>() { TokenObjectType.Italic, TokenObjectType.Strong } }
        };

        private static readonly Dictionary<TokenObjectType, Func<List<Token>, int, int, bool>> objectTypeValidators = new Dictionary<TokenObjectType, Func<List<Token>, int, int, bool>>()
        {
            { TokenObjectType.Italic, IsItalicValid },
            { TokenObjectType.Strong, IsStrongValid },
            { TokenObjectType.Header, IsHeaderValid }
        };

        public static List<Token> ValidateObjectTokens(List<Token> tokens)
        {
            for (var openTokenIndex = 0; openTokenIndex < tokens.Count; openTokenIndex++)
            {
                var currentToken = tokens[openTokenIndex] as ObjectToken;
                if (currentToken == null || currentToken.IsClose)
                    continue;

                var closeTokenIndex = tokens.FindIndex(openTokenIndex,
                                                         token => token is ObjectToken &&
                                                                  (token as ObjectToken)!.IsClose &&
                                                                  (token as ObjectToken)!.ObjectType == currentToken.ObjectType);
                if (closeTokenIndex < 0)
                {
                    tokens[openTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                    continue;
                }

                if (objectTypeValidators[currentToken.ObjectType](tokens, openTokenIndex, closeTokenIndex))
                    continue;

                var closingToken = tokens[closeTokenIndex];
                tokens[openTokenIndex] = new Token() { Type = TokenType.Text, Text = currentToken.Text };
                tokens[closeTokenIndex] = new Token() { Type = TokenType.Text, Text = closingToken.Text };
            }

            return tokens;
        }

        private static bool IsStrongValid(List<Token> tokens, int openTokenIndex, int closeTokenIndex)
        {
            return closeTokenIndex - openTokenIndex > 1;
        }

        private static bool IsHeaderValid(List<Token> tokens, int openTokenIndex, int closeTokenIndex)
        {
            return openTokenIndex == 0 || tokens[openTokenIndex - 1].Type == TokenType.Newline;
        }

        private static bool IsItalicValid(List<Token> tokens, int openTokenIndex, int closeTokenIndex)
        {
            var hasSpaceInsideOnTheLeftOrRight = tokens[openTokenIndex + 1].Type == TokenType.Space ||
                                                 tokens[closeTokenIndex - 1].Type == TokenType.Space;
            if (hasSpaceInsideOnTheLeftOrRight)
                return false;

            var hasTextOnTheLeft = openTokenIndex > 0 && tokens[openTokenIndex - 1].Type == TokenType.Text;
            var hasTextOnTheRight = closeTokenIndex < tokens.Count - 1 &&
                                    tokens[closeTokenIndex + 1].Type == TokenType.Text;
            var hasSpacesBetween = tokens.FindIndex(openTokenIndex,
                                                    closeTokenIndex - openTokenIndex + 1,
                                                    token => token.Type == TokenType.Space) > 0;
            if (openTokenIndex == 0 && !(hasSpacesBetween && hasTextOnTheRight) ||
                closeTokenIndex == tokens.Count - 1 && !(hasTextOnTheLeft && hasSpacesBetween))
            {
                return true;
            }

            if (hasTextOnTheLeft && hasTextOnTheRight && hasSpacesBetween ||
                !hasTextOnTheLeft && hasTextOnTheRight ||
                hasTextOnTheLeft && !hasTextOnTheRight)
            {
                return false;
            }

            if (!hasTextOnTheLeft && !hasTextOnTheRight)
                return true;

            var hasNotOnlyNumbersInside = tokens.FindIndex(openTokenIndex,
                                                               closeTokenIndex - openTokenIndex + 1,
                                                               token => token.Type == TokenType.Text &&
                                                                        token.Text.Any(c => !char.IsDigit(c))) >= 0;
            return hasNotOnlyNumbersInside;
        }

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