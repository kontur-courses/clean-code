using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class ElementsFinder
    {
        public static IEnumerable<Element> FindElements(string text)
        {
            var headingElements = FindHeadingElements(text);
            var boldElements = FindBoldElements(text);
            var italicElements = FindItalicElements(text);
            DeleteBoldInItalic(boldElements, italicElements);
            DeleteBoldAndItalicIntersection(boldElements, italicElements);
            return headingElements.Concat(boldElements).Concat(italicElements);
        }
        private static IEnumerable<Element> FindHeadingElements(string text)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Element> FindBoldElements(string text)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Element> FindItalicElements(string text)
        {
            throw new NotImplementedException();
        }

        private static void DeleteBoldInItalic(IEnumerable<Element> bold, IEnumerable<Element> italic)
        {
            throw new NotImplementedException();
        }

        private static void DeleteBoldAndItalicIntersection(IEnumerable<Element> bold, IEnumerable<Element> italic)
        {
            throw new NotImplementedException();
        }
    }
}
