using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers;

namespace Markdown
{
    public class TokenTreeCreator
    {
        private readonly List<IParser> parsers;

        public TokenTreeCreator(params IParser[] parsers)
        {
            this.parsers = parsers.ToList();
        }

        /// <summary>
        /// Метод, который парсит текст и создаёт дерево токенов оформления
        /// </summary>
        /// <param name="text">Текст, который необходимо распарсить</param>
        /// <returns>Корневой токен полученного дерева</returns>
        public Token GetRootToken(string text)
        {
            var closeTagPositions = new SortedStack<(int, int)>();
            var tokensStack = new Stack<Token>();
            var rootToken = new Token(0, text.Length, MdTags.Default, TextType.Default);
            SaveToken(rootToken, tokensStack, closeTagPositions);

            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\')
                {
                    i++;
                    continue;
                }
                if (char.IsLetter(text[i])) continue;
                if (text[i] == ' ') continue;
                if (i == closeTagPositions.Peek().Item1)
                {
                    i += closeTagPositions.Pop().Item2;
                    continue;
                }

                var token = TryParseToken(i, text);
                if (token == null) continue;
                CheckTokenAndTrySaveThem(token, tokensStack, closeTagPositions);
                i += token.Tag.Open.Length - 1;
            }

            return rootToken;
        }

        private static void CheckTokenAndTrySaveThem(
            Token token, Stack<Token> tokensStack, SortedStack<(int, int)> closeTagPositions)
        {
            while (tokensStack.Count > 0 && token.Position > tokensStack.Peek().NextIndex)
                tokensStack.Pop();
            
            if (tokensStack.Count == 0) throw new Exception("Not found root token");
            if (IntersectionRemoved(token, tokensStack)) return;
            if (AttachmentIsForbidden(token, tokensStack.Peek())) return;

            tokensStack.Peek().AddInternalToken(token);
            SaveToken(token, tokensStack, closeTagPositions);
        }

        private static void SaveToken(Token token, Stack<Token> tokensStack, SortedStack<(int, int)> closeTagPositions)
        {
            tokensStack.Push(token);
            closeTagPositions.Push((
                token.NextIndex - token.Tag.Close.Length,
                token.Tag.Close.Length));
        }

        private static bool AttachmentIsForbidden(Token currentToken, Token parentToken)
        {
            return currentToken.Type == TextType.Bold && parentToken.Type == TextType.Italic;
        }

        private static bool IntersectionRemoved(Token currentToken, Stack<Token> tokensStack)
        {
            if (currentToken.NextIndex <= tokensStack.Peek().NextIndex) return false;
            var tokenToRemove = tokensStack.Pop();
            tokensStack.Peek().RemoveInternalToken(tokenToRemove);
            return true;

        }

        private Token TryParseToken(int index, string text)
        {
            return parsers
                .Select(handler => handler
                    .TryHandleTag(index, text))
                .FirstOrDefault(token => token != null);
        }
    }
}