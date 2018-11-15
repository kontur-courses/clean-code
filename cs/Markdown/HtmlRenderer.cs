using System;
using System.Collections.Generic;
using Markdown.Elements;
using System.Linq;

namespace Markdown
{
    public class HtmlRenderer
    {
        private static readonly Dictionary<IElementType, (string open, string close)> htmlTagsMapping =
            new Dictionary<IElementType, (string open, string close)>
                {
                    { RootElementType.Create(), (open: "", close: "") },
                    { UnderscoreElementType.Create(), (open: "<em>", close: "</em>") },
                    { DoubleUnderscoreElementType.Create(), (open: "<strong>", close: "</strong>") }
                };
    
        public static string RenderToHtml(MarkdownElement markdownRoot)
        {
            var currentPosition = markdownRoot.StartPosition;
            var result = "";

            foreach (var innerElement in markdownRoot.InnerElements)
            {
                var innerElementIndicatorStart = innerElement.StartPosition - innerElement.ElementType.Indicator.Length;
                var partBeforeInnerElement = markdownRoot.Markdown.Substring(
                    currentPosition, innerElementIndicatorStart - currentPosition);

                var innerElementRenderResult = RenderToHtml(innerElement);

                result += partBeforeInnerElement + innerElementRenderResult;
                currentPosition = innerElement.EndPosition + innerElement.ElementType.Indicator.Length;
            }

            var remainder = markdownRoot.Markdown.Substring(
                currentPosition, markdownRoot.EndPosition - currentPosition);
            result += remainder;

            return WrapString(result, markdownRoot.ElementType);
        }

        private static string WrapString(string str, IElementType element)
        {
            return $"{htmlTagsMapping[element].open}{str}{htmlTagsMapping[element].close}";
        }
    }
}
