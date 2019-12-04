using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownProcessor.Wraps;

namespace MarkdownProcessor
{
    public static class TextRenderer
    {
        public static string Render(IEnumerable<Token> tokens, Func<IWrapType, IWrapType> wrapTypeConverter)
        {
            var textBuilder = new StringBuilder();

            foreach (var token in tokens)
            {
                textBuilder.Append(wrapTypeConverter(token.WrapType).OpenWrapMarker);
                textBuilder.Append(ComposeAllChildTokens(token, wrapTypeConverter));
                textBuilder.Append(wrapTypeConverter(token.WrapType).CloseWrapMarker);
            }

            return textBuilder.ToString();
        }

        private static string ComposeAllChildTokens(Token rootToken, Func<IWrapType, IWrapType> wrapTypeConverter)
        {
            var childTokensBuilder = new StringBuilder(rootToken.Content.Length);

            var allChildTokens = GetAllChildTokens(rootToken).Reverse().ToArray();

            var currentChildTokenIndex = 0;
            var position = 0;

            while (position < rootToken.Content.Length)
            {
                var currentChildToken = TryGetCurrentChildToken(currentChildTokenIndex, allChildTokens);

                if (position + rootToken.ContentStartIndex == currentChildToken?.ContentStartIndex)
                {
                    RemoveLastCharacters(currentChildToken.WrapType.OpenWrapMarker.Length, childTokensBuilder);
                    childTokensBuilder.Append(wrapTypeConverter(currentChildToken.WrapType).OpenWrapMarker);
                }

                if (position == currentChildToken?.ContentEndIndex)
                {
                    RemoveLastCharacters(currentChildToken.WrapType.CloseWrapMarker.Length, childTokensBuilder);
                    childTokensBuilder.Append(wrapTypeConverter(currentChildToken.WrapType).CloseWrapMarker);
                    currentChildTokenIndex++;
                }

                childTokensBuilder.Append(rootToken.Content[position]);
                position++;
            }

            return childTokensBuilder.ToString();
        }

        private static IEnumerable<Token> GetAllChildTokens(Token rootToken)
        {
            var tokensStack = new Stack<Token>();
            tokensStack.Push(rootToken);

            while (tokensStack.Count != 0)
            {
                var currentToken = tokensStack.Pop();

                if (currentToken.ChildTokens != null)
                    foreach (var childToken in currentToken.ChildTokens)
                        tokensStack.Push(childToken);

                if (currentToken != rootToken)
                    yield return currentToken;
            }
        }

        private static Token TryGetCurrentChildToken(int currentChildTokenIndex, IReadOnlyList<Token> allChildTokens) =>
            currentChildTokenIndex < allChildTokens.Count
                ? allChildTokens[currentChildTokenIndex]
                : null;

        private static void RemoveLastCharacters(int count, StringBuilder stringBuilder) =>
            stringBuilder.Remove(stringBuilder.Length - count, count);
    }
}