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
                if (position + rootToken.ContentStartIndex == TryGetCurrentChildToken()?.ContentStartIndex)
                {
                    RemoveLastCharacters(TryGetCurrentChildToken().WrapType.OpenWrapMarker.Length);
                    childTokensBuilder.Append(wrapTypeConverter(TryGetCurrentChildToken().WrapType).OpenWrapMarker);
                }

                if (position == TryGetCurrentChildToken()?.ContentEndIndex)
                {
                    RemoveLastCharacters(TryGetCurrentChildToken().WrapType.CloseWrapMarker.Length);
                    childTokensBuilder.Append(wrapTypeConverter(TryGetCurrentChildToken().WrapType).CloseWrapMarker);
                    currentChildTokenIndex++;
                }

                childTokensBuilder.Append(rootToken.Content[position]);
                position++;
            }

            return childTokensBuilder.ToString();

            Token TryGetCurrentChildToken() => currentChildTokenIndex < allChildTokens.Length
                                                   ? allChildTokens[currentChildTokenIndex]
                                                   : null;

            void RemoveLastCharacters(int count) => childTokensBuilder.Remove(childTokensBuilder.Length - count, count);
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
    }
}