using System;
using System.Collections.Generic;
using Markdown.Elements;

namespace Markdown.Parsers
{
    public class EmphasisParser
    {


        public static ParsingResult ParseElement(string markdown, int startPosition, IElementType elementType)
        {
            int currentPosition = startPosition;
            while (currentPosition < markdown.Length && 
                   !IsClosingOfElement(markdown, currentPosition, elementType))
                currentPosition++;

            return new ParsingResult(true,
                new MarkdownElement(elementType, markdown, startPosition, currentPosition, new List<MarkdownElement>()));
        }

        private static bool IsOpeningOfElement(string markdown, int position, IElementType elementType)
        {
            if (markdown.IsEscapedCharAt(position))
                return false;

            int positionAfterIndicator = position + elementType.Indicator.Length;
            return elementType.IsIndicatorAt(markdown, position) &&
                   positionAfterIndicator < markdown.Length &&
                   !Char.IsWhiteSpace(markdown[positionAfterIndicator]);
        }

        private static bool IsClosingOfElement(string markdown, int position, IElementType elementType)
        {
            if (markdown.IsEscapedCharAt(position))
                return false;

            return elementType.IsIndicatorAt(markdown, position) &&
                   position >= 1 && !Char.IsWhiteSpace(markdown[position - 1]);
        }
    }
}
