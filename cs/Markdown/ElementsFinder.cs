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
                if (text.IsStartOfParagraph(startTagPosition))
                {
                    pointer = text.GetEndOfParagraphPosition(pointer);
                    yield return Element.Create(Md.HeadingStyle, startTagPosition, pointer);
                }
            }
        }

        private static IEnumerable<Element> FindBoldElements(string text)
        {
            return FindBoldOrItalicElements(text, Md.BoldStyle);
        }

        private static IEnumerable<Element> FindItalicElements(string text)
        {
            return FindBoldOrItalicElements(text, Md.ItalicStyle);
        }

        private static IEnumerable<Element> FindBoldOrItalicElements(string text, Style style)
        {
            var pointer = 0;
            var startTag = style.StartTag;
            var endTag = style.EndTag;
            var startTagPosition = text.IndexOf(startTag, pointer);
            pointer = startTagPosition + startTag.Length;
            var endTagPosition = text.IndexOf(endTag, pointer);
            pointer = endTagPosition + endTag.Length;

            while (true)
            {
                if (startTagPosition == -1 || endTagPosition == -1)
                    break;

                if (IsEmptyStringInside(style, startTagPosition, endTagPosition))
                {
                    startTagPosition = text.IndexOf(startTag, pointer);
                    pointer = startTagPosition + startTag.Length;
                    endTagPosition = text.IndexOf(endTag, pointer);
                    pointer = endTagPosition + endTag.Length;
                    continue;
                }

                if (!IsBoldOrItalicStartTag(text, startTagPosition, style))
                {
                    startTagPosition = endTagPosition;
                    endTagPosition = text.IndexOf(endTag, pointer);
                    pointer = endTagPosition + endTag.Length;
                    continue;
                }

                if (!IsBoldOrItalicEndTag(text, endTagPosition, style))
                {
                    endTagPosition = text.IndexOf(endTag, pointer);
                    pointer = endTagPosition + endTag.Length;
                    continue;
                }

                yield return Element.Create(style, startTagPosition, endTagPosition);

                startTagPosition = text.IndexOf(startTag, pointer);
                pointer = startTagPosition + startTag.Length;
                endTagPosition = text.IndexOf(endTag, pointer);
                pointer = endTagPosition + endTag.Length;
            }
        }

        private static bool IsBoldOrItalicStartTag(string text, int startTagPosition, Style style)
        {
            var word = text.GetWordContainingCurrentSymbol(startTagPosition);
            if (word.IsInside(style.StartTag, startTagPosition) && word.ContainsDigit())
                return false;
            return true;
        }

        private static bool IsBoldOrItalicEndTag(string text, int endTagPosition, Style style)
        {
            var word = text.GetWordContainingCurrentSymbol(endTagPosition);
            if (word.IsInside(style.StartTag, endTagPosition) && word.ContainsDigit())
                return false;
            return true;
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

        private static bool IsEmptyStringInside(Style style, int startTagPosition, int endTagPosition)
        {
            return endTagPosition - startTagPosition - style.StartTag.Length == 0;
        }
    }
}
