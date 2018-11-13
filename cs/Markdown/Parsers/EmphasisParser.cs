using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.Elements;

namespace Markdown.Parsers
{
    public class EmphasisParser
    {
        private static readonly IElementType[] PossibleElementTypes = new IElementType[]
        {
            UnderscoreElementType.Create(), DoubleUnderscoreElementType.Create()
        };

        public static MarkdownElement ParseElement(string markdown, int startPosition, IElementType elementType)
        {
            var currentPosition = startPosition;
            var innerElements = new List<MarkdownElement>();

            while (currentPosition < markdown.Length)
            {
                if (IsClosingOfElement(markdown, currentPosition, elementType))
                    return new MarkdownElement(
                        elementType, markdown, startPosition, currentPosition, innerElements);

                var openingElementType = GetOpeningElementType(markdown, currentPosition);
                var closingElementType = GetClosingElementType(markdown, currentPosition);
                
                if (openingElementType != null && elementType.CanContainElement(openingElementType))
                {
                    var innerElement = ParseElement(
                        markdown,
                        currentPosition + openingElementType.Indicator.Length,
                        openingElementType);

                    if (innerElement.ElementType == BrokenElementType.Create())
                        innerElements = innerElements.Concat(innerElement.InnerElements).ToList();
                    else
                        innerElements.Add(innerElement);
                    currentPosition = innerElement.EndPosition + innerElement.ElementType.Indicator.Length;
                }  
                else if (openingElementType != null || 
                         closingElementType != null && elementType != RootElementType.Create())
                {
                    return new MarkdownElement(
                        BrokenElementType.Create(), markdown, startPosition, currentPosition, innerElements);
                }
                else
                    currentPosition++;
            }

            var isBrokenElement = elementType != RootElementType.Create();
            
            return new MarkdownElement(
                isBrokenElement ? BrokenElementType.Create() : elementType, 
                markdown, startPosition, currentPosition, innerElements);
        }

        private static IElementType GetOpeningElementType(string markdown, int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => IsOpeningOfElement(markdown, position, type));
        }

        private static IElementType GetClosingElementType(string markdown, int position)
        {
            return PossibleElementTypes
                .FirstOrDefault(type => IsClosingOfElement(markdown, position, type));
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
