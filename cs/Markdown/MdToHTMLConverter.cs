using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown
{
    class MdToHTMLConverter
    {
        private static readonly Dictionary<Style, Style> convertationTable = new Dictionary<Style, Style>
        {
            [Md.HeadingStyle] = HTML.HeadingStyle,
            [Md.BoldStyle] = HTML.BoldStyle,
            [Md.ItalicStyle] = HTML.ItalicStyle
        };

        public static string Convert(string mdText)
        {
            var mdElements = ElementsFinder.FindElements(mdText);
            var htmlElements = ConvertElements(mdElements);
            return ElementsToHTMLText(mdText, htmlElements.ToList());
        }

        private static IEnumerable<Element> ConvertElements(IEnumerable<Element> elements)
        {
            foreach (var element in elements)
            {
                yield return new Element(
                    HTML.htmlStyles[element.ElementStyle.Type],
                    element.ElementStart,
                    element.ElementLength,
                    element.ContentStart,
                    element.ContentLength);
            }
        }

        private static void DeleteBackSlashBeforeMdTags(string text)
        {
            throw new NotImplementedException();
        }

        private static string ElementsToHTMLText(string mdText, List<Element> elements)
        {
            var htmlText = new StringBuilder();
            var ptr = 0;
            while (true)
            {
                var elementStartingInPtr = elements.Where(e => e.ElementStart == ptr).ToList();
                if (elementStartingInPtr.Count != 0)
                {
                    htmlText.Append(elementStartingInPtr[0].ElementStyle.StartTag);
                    ptr = elementStartingInPtr[0].ContentStart;
                    continue;
                }
                var elementEndingInPtr = elements.Where(e => e.ContentStart + e.ContentLength == ptr).OrderBy(e => e.ElementStart).ToList();
                if (elementEndingInPtr.Count != 0)
                {
                    elements.Remove(elementEndingInPtr[0]);
                    htmlText.Append(elementEndingInPtr[0].ElementStyle.EndTag);
                    ptr = elementEndingInPtr[0].ElementStart + elementEndingInPtr[0].ElementLength;
                    continue;
                }
                if (ptr >= mdText.Length)
                    break;
                htmlText.Append(mdText[ptr]);
                ptr++;
            }
            return htmlText.ToString();
        }
    }
}
