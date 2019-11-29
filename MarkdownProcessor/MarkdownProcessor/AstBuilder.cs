using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Wraps;

namespace MarkdownProcessor
{
    public static class AstBuilder
    {
        public static string BuildAst(
            IEnumerable<Token> tokens, IReadOnlyDictionary<IWrapType, IWrapType> htmlWrapByMarkdownWrap)
        {
            var treeBuilder = new StringBuilder();

            foreach (var token in tokens)
                treeBuilder.Append(BuildSubtree(token, htmlWrapByMarkdownWrap));

            return treeBuilder.ToString();
        }

        private static string BuildSubtree(Token rootToken,
                                           IReadOnlyDictionary<IWrapType, IWrapType> htmlWrapByMarkdownWrap)
        {
            var subTreeBuilder = new StringBuilder(rootToken.Content.Length);

            subTreeBuilder.Append(htmlWrapByMarkdownWrap[rootToken.WrapType].OpenWrapMarker);

            var allChildTokens = GetAllChildTokens(rootToken).Reverse().ToArray();

            var currentChildTokenIndex = 0;
            var position = 0;

            while (position < rootToken.Content.Length)
            {
                if (position + rootToken.ContentStartIndex == TryGetCurrentChildToken()?.ContentStartIndex)
                {
                    RemoveLastCharacters(TryGetCurrentChildToken().WrapType.OpenWrapMarker.Length);
                    subTreeBuilder.Append(htmlWrapByMarkdownWrap[TryGetCurrentChildToken().WrapType].OpenWrapMarker);
                }

                if (position == TryGetCurrentChildToken()?.ContentEndIndex)
                {
                    RemoveLastCharacters(TryGetCurrentChildToken().WrapType.CloseWrapMarker.Length);
                    subTreeBuilder.Append(htmlWrapByMarkdownWrap[TryGetCurrentChildToken().WrapType].CloseWrapMarker);
                    currentChildTokenIndex++;
                }

                subTreeBuilder.Append(rootToken.Content[position]);
                position++;
            }

            subTreeBuilder.Append(htmlWrapByMarkdownWrap[rootToken.WrapType].CloseWrapMarker);

            return subTreeBuilder.ToString();

            Token TryGetCurrentChildToken() => currentChildTokenIndex < allChildTokens.Length
                                                   ? allChildTokens[currentChildTokenIndex]
                                                   : null;

            void RemoveLastCharacters(int count) => subTreeBuilder.Remove(subTreeBuilder.Length - count, count);
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