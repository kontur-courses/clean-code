using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Element;

namespace Markdown
{
    public static class ResultFormatter
    {
        public static string Form(IEnumerable<Token.Token> tokens)
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

        public static string PrepareResult(string str)
        {
            var emHtmlElement = new HtmlElement("_", "<em>");
            var strongHtmlElement = new HtmlElement("__", "<strong>");


            var emOpen = str.AllIndexesOf(emHtmlElement.OpenTag);
            var emClose = str.AllIndexesOf(emHtmlElement.ClosingTag);

            var strongOpen = str.AllIndexesOf(strongHtmlElement.OpenTag);
            var strongClose = str.AllIndexesOf(strongHtmlElement.ClosingTag);

            if (strongOpen.Count > 0)
            {
                for (var index = 0; index < emOpen.Count; index++)
                {

                    if (index < strongClose.Count && emOpen[index] < strongOpen[index] && emClose[index] > strongClose[index])
                    {
                        str = str.Replace(strongHtmlElement.OpenTag, "__").Replace(strongHtmlElement.ClosingTag, "__");
                    }
                }
            }


            return str.Replace(@"\", "");
        }

        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            var indexes = new List<int>();
            for (var index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index, StringComparison.Ordinal);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}
