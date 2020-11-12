using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal static class ElementsFinder
    {
        public static IEnumerable<Element> FindElements(string text)
        {
            var headingFinder = new MdHeadingStyleFinder(Md.HeadingStyle, text);
            var headingElements = headingFinder.Find();
            var boldFinder = new MdBoldStyleFinder(Md.BoldStyle, text);
            var boldElements = boldFinder.Find().ToList();
            var italicFinder = new MdItalicStyleFinder(Md.ItalicStyle, text, Md.BoldStyle);
            var italicElements = italicFinder.Find().ToList();
            DeleteBoldInItalic(boldElements, italicElements);
            DeleteBoldAndItalicIntersection(boldElements, italicElements);
            return headingElements.Concat(boldElements).Concat(italicElements);
        }

        private static void DeleteBoldInItalic(List<Element> bold, List<Element> italic)
        {
            foreach (var italicElement in italic)
            foreach (var boldElement in bold.ToList())
                if (boldElement.ElementStart > italicElement.ElementStart
                    && boldElement.ElementStart + boldElement.ElementLength <
                    italicElement.ElementStart + italicElement.ElementLength)
                    bold.Remove(boldElement);
        }

        private static void DeleteBoldAndItalicIntersection(List<Element> bold, List<Element> italic)
        {
            foreach (var boldElement in bold.ToList())
            foreach (var italicElement in italic.ToList())
                if (boldElement.IntersectsWith(italicElement))
                {
                    italic.Remove(italicElement);
                    bold.Remove(boldElement);
                    break;
                }
        }
    }
}