using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    static class ElementsFinder
    {
        public static IEnumerable<Element> FindElements(string text)
        {
            var headingElements = FindHeadingElements(text).ToList();
            var boldElements = FindBoldElements(text).ToList();
            var italicElements = FindItalicElements(text).ToList();
            DeleteBoldInItalic(boldElements, italicElements);
            DeleteBoldAndItalicIntersection(boldElements, italicElements);
            return headingElements.Concat(boldElements).Concat(italicElements);
        }
        private static IEnumerable<Element> FindHeadingElements(string text)
        {
            var pointer = 0;
            var startTag = Md.HeadingStyle.StartTag;
            int startTagPosition;
            while ((startTagPosition = text.IndexOf(startTag, pointer)) != -1)
            {
                pointer = startTagPosition + startTag.Length;
                if (IsStartOfParagraph(text, startTagPosition))
                {
                    pointer = GetEndOfParagraphPosition(text, pointer);
                    yield return GetElement(Md.HeadingStyle, startTagPosition, pointer);
                }
            }
        }

        private static Element GetElement(Style style, int startTagPosition, int endTagPosition)
        {
            return new Element(
                style,
                startTagPosition,
                endTagPosition + style.EndTag.Length - startTagPosition,
                startTagPosition + style.StartTag.Length,
                endTagPosition - startTagPosition - style.StartTag.Length);
        }

        private static bool IsStartOfParagraph(string text, int pointer)
        {
            return pointer == 0 || text[pointer - 1] == '\n';
        }

        private static bool IsEndOfParagraph(string text, int pointer)
        {
            return pointer == text.Length || text[pointer] == '\n';
        }

        private static int GetEndOfParagraphPosition(string text, int pointer)
        {
            while (!IsEndOfParagraph(text, pointer))
            {
                pointer++;
            }
            return pointer;
        }

        private static IEnumerable<Element> FindBoldElements(string text)
        {
            var pointer = 0;
            int startTagPosition;
            var startTag = Md.BoldStyle.StartTag;
            var endTag = Md.BoldStyle.EndTag;
            while ((startTagPosition = text.IndexOf(startTag, pointer)) != -1)
            {
                pointer = startTagPosition + startTag.Length;
                var endTagPosition = text.IndexOf(endTag, pointer);
                if (endTagPosition == -1)
                    break;
                yield return GetElement(Md.BoldStyle, startTagPosition, endTagPosition);
                pointer = endTagPosition + endTag.Length;
            }
        }

        private static IEnumerable<Element> FindItalicElements(string text)
        {
            var pointer = 0;
            int startTagPosition;
            var startTag = Md.ItalicStyle.StartTag;
            var endTag = Md.ItalicStyle.EndTag;
            while ((startTagPosition = text.IndexOf(Md.ItalicStyle.StartTag, pointer)) != -1)
            {
                pointer = startTagPosition + startTag.Length;
                var endTagPosition = text.IndexOf(endTag, pointer);
                if (endTagPosition == -1)
                    break;
                if (endTagPosition - startTagPosition == 1)
                {
                    pointer = endTagPosition + endTag.Length;
                    continue;
                }
                yield return GetElement(Md.ItalicStyle, startTagPosition, endTagPosition);
                pointer = endTagPosition + endTag.Length;
            }
        }

        private static void DeleteBoldInItalic(List<Element> bold, List<Element> italic)
        {
            foreach (var italicElement in italic)
            {
                foreach (var boldElement in bold.ToList())
                {
                    if (boldElement.ElementStart > italicElement.ElementStart
                        && boldElement.ElementStart + boldElement.ElementLength < italicElement.ElementStart + italicElement.ElementLength)
                        bold.Remove(boldElement);
                }
            }
        }

        private static void DeleteBoldAndItalicIntersection(IEnumerable<Element> bold, IEnumerable<Element> italic)
        {
            return;
        }
    }
}
