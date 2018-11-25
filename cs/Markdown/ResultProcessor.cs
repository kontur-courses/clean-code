using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Element;
using Markdown.Extensions;
using Markdown.Token;

namespace Markdown
{
    public static class ResultProcessor
    {
        public static string ProcessResult(string text, IEnumerable<IElement> elements)
        {
            text = elements.Aggregate(text, (current, element)
                => Form(TokenParser.Parse(current, element)));

            return PrepareResult(text);
        }

        private static string Form(IEnumerable<Token.Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                result
                    .Append(token.Element.OpenTag)
                    .Append(token.Content)
                    .Append(token.Element.ClosingTag);
            }

            return result.ToString();
        }

        private static string PrepareResult(string str)
        {
            var emHtmlElement = new HtmlElement("_", "em");
            var strongHtmlElement = new HtmlElement("__", "strong");

            var emOpen = str.AllIndexesOf(emHtmlElement.OpenTag);
            var emClose = str.AllIndexesOf(emHtmlElement.ClosingTag);

            var strongOpen = str.AllIndexesOf(strongHtmlElement.OpenTag);
            var strongClose = str.AllIndexesOf(strongHtmlElement.ClosingTag);

            if (strongOpen.Count <= 0)
                return str.RemoveEscapeSymbols();

            var s = new StringBuilder(str);

            for (var index = 0; index < emOpen.Count; index++)
            {
                if (index >= strongClose.Count || emOpen[index] >= strongOpen[index] ||
                    emClose[index] <= strongClose[index])
                    continue;

                s.Remove(strongClose[index], strongHtmlElement.ClosingTag.Length);
                s.Insert(strongClose[index], strongHtmlElement.Indicator);

                s.Remove(strongOpen[index], strongHtmlElement.OpenTag.Length);
                s.Insert(strongOpen[index], strongHtmlElement.Indicator);
            }

            return s.ToString().RemoveEscapeSymbols();
        }
    }
}
