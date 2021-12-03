using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Renderer
{
    public class HtmlRenderer : IRenderer
    {
        private HashSet<MarkdownToken> visitedTokens = new();

        public string Render(MarkdownToken[] tokens)
        {
            var renderedTokens = new List<string>();
            var sortedTokens = tokens
                .OrderBy(t => t.ParagraphIndex)
                .ThenBy(t => t.StartIndex)
                .ToArray();
            for (var i = 0; i < sortedTokens.Length; i++)
            {
                if(!visitedTokens.Contains(tokens[i]))
                        renderedTokens.Add(RenderToken(sortedTokens, i));
            }

            return string.Join("", renderedTokens);
        }

        private string RenderToken(MarkdownToken[] tokens, int i)
        {
            visitedTokens.Add(tokens[i]);
            if (i == tokens.Length - 1 || !NextTokenIsNested(tokens[i], tokens[i + 1]))
                return tokens[i].GetHtmlFormatted();
            var currentToken = tokens[i];
            var currentValue = currentToken.GetHtmlFormatted();
            var nextToken = tokens[i + 1];
            var replacementShift = currentToken.OpenHtmlTag.Length - currentToken.Selector.Length;
            var startInsertIndex = nextToken.StartIndex - currentToken.StartIndex + replacementShift;
            var finishInsertIndex = startInsertIndex + nextToken.Length;
            return $"{currentValue[..startInsertIndex]}{RenderToken(tokens, i + 1)}{currentValue[finishInsertIndex..]}";
        }

    private bool NextTokenIsNested(MarkdownToken current, MarkdownToken next) 
            => next.StartIndex > current.StartIndex && next.FinishIndex <= current.FinishIndex;
    }
}