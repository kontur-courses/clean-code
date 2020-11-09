using System;
using System.Collections.Generic;
using System.Text;

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
            return ElementsToHTMLText(mdText, htmlElements);
        }

        private static IEnumerable<Element> ConvertElements(IEnumerable<Element> elements)
        {
            throw new NotImplementedException();
        }

        private static void DeleteBackSlashBeforeMdTags(string text)
        {
            throw new NotImplementedException();
        }

        private static string ElementsToHTMLText(string mdText, IEnumerable<Element> elements)
        {
            throw new NotImplementedException();
        }
    }
}
